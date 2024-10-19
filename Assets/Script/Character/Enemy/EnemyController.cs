using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyController : Controller
{
    [SerializeField] private GameObject player;
    private Controller playerController;
    private Vector3 direction;
    private Vector2 input;
    private bool isTriggered = false;
    private Vector2 patrolStartPos;
    private Vector2 patrolEndPos;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] int patrolRange = 10;
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] float maxComboAttackGap = 2.5f;
    private float nextAttackTime;
    [SerializeField] float hurtDuration = 1.5f;
    string[] attackPool = { "A", "B", "C" };
    string comboSequence = "";
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<Controller>();
        Assert.IsNotNull(player);
        Assert.IsNotNull(playerController);
        input = Vector2.zero;
        direction = Vector3.zero;
        patrolStartPos = transform.position;
        patrolEndPos = transform.position + new Vector3(patrolRange, 0, 0);
        nextAttackTime = 0f;
        isTriggered = false;

        _command.RegisterPattern(new ComboAttackPattern(new[] { "A" }, 0, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new[] { "A", "A" }, 1, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new[] { "A", "A", "A" }, 2, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new[] { "B" }, 0, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new[] { "B", "B" }, 1, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new[] { "C" }, 0, maxComboAttackGap));
    }

    protected override void OnShadowTriggerEnter(Collider2D other)
    {
        base.OnShadowTriggerEnter(other);
        Debug.Log($"OnShadowTriggerEnter: " + other.name);
        Controller controller = other.GetComponentInParent<PlayerController>();
        if (controller != null) isTriggered = true;
    }

    protected override void OnShadowTriggerExit(Collider2D other)
    {
        base.OnShadowTriggerExit(other);
        Debug.Log($"OnShadowTriggerExit: " + other.name);
        PlayerController controller = other.GetComponentInParent<PlayerController>();
        if (controller != null) isTriggered = false;
        _command.ExecuteCommand(new MoveCommand(input));
    }

    private void Start()
    {
        _command.ExecuteCommand(new IdleCommand());
    }

    private void FixedUpdate()
    {
        if (isTriggered && !playerController.isDead)
        {
            if (Time.time > nextAttackTime)
            {
                PrepareAttack();
            }
        }
        else if (ifPlayerIsNear())
        {
            ChasePlayer();
        }
        else Patrol();
    }

    private void PrepareAttack()
    {

        if (comboSequence.Contains("A") && comboSequence.Length < 3) PerformAttack("A");
        else if (comboSequence.Contains("B") && comboSequence.Length < 2) PerformAttack("B");
        else PerformAttack(DrawAttack());

    }

    private string DrawAttack()
    {
        string avoid = comboSequence.Substring(0);
        comboSequence = "";
        List<string> newPool = new List<string>();

        foreach (var attack in attackPool)
            if (!avoid.Contains(attack)) newPool.Add(attack);

        return newPool[UnityEngine.Random.Range(0, newPool.Count)];
    }

    private bool ifPlayerIsNear()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < chaseRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ChasePlayer()
    {
        direction = player.transform.position - transform.position;
        direction.Normalize();
        if (input != new Vector2(direction.x, direction.y) && !isTriggered)
        {
            input = new Vector2(direction.x, direction.y);
            _command.ExecuteCommand(new MoveCommand(input));
        }
        else
        {
            _command.ExecuteCommand(new MoveCommand(input));
        }
    }

    private void Patrol()
    {
        if (transform.position.x <= patrolStartPos.x)
        {
            input = new Vector2(1, 0);
            _command.ExecuteCommand(new MoveCommand(input));
        }
        else if (transform.position.x >= patrolEndPos.x)
        {
            input = new Vector2(-1, 0);
            _command.ExecuteCommand(new MoveCommand(input));
        }
    }

    public void PerformAttack(string pattern)
    {
        comboSequence += pattern;
        _command.ExecuteCommand(new AttackCommand(pattern));
        nextAttackTime = Time.time + attackCooldown;
    }

    public override bool Attack(AttackConfig config)
    {
        var triggers = _shadow.Triggers;
        var targetHitCommand = config?.targetHitAction.GetCommand();

        for (var i = 0; i < triggers.Count; i++)
        {
            var player = triggers[i].GetComponentInParent<PlayerController>();
            if (player == null)
                continue;

            if (Math.Sign(LastFaceDirection) == Math.Sign(player.transform.position.x - transform.position.x))
            {
                player.Hurt(attackDamage);
                if (targetHitCommand != null)
                {
                    player.Command.ExecuteCommand(targetHitCommand);
                }
                return true;
            }
        }

        return false;
    }
}
