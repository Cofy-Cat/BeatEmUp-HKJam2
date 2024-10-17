using System;
using NUnit.Framework;
using UnityEngine;

public class EnemyController : Controller
{
    private Controller player;
    private Vector3 direction;
    private Vector2 input;
    private bool isTriggered = false;
    private Vector2 patrolStartPos;
    private Vector2 patrolEndPos;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] int patrolRange = 10;
    [SerializeField] float attackCooldown = 1f;
    private float nextAttackTime;
    [SerializeField] float hurtDuration = 1.5f;
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player").GetComponent<Controller>();
        Assert.NotNull(player);
        input = Vector2.zero;
        direction = Vector3.zero;
        patrolStartPos = transform.position;
        patrolEndPos = transform.position + new Vector3(patrolRange, 0, 0);
        nextAttackTime = 0f;
    }
    
    protected override void OnShadowTriggerEnter(Collider2D other)
    {
        base.OnShadowTriggerEnter(other);
        Debug.Log($"OnShadowTriggerEnter: " + other.name);
        isTriggered = true;
    }

    protected override void OnShadowTriggerExit(Collider2D other)
    {
        base.OnShadowTriggerExit(other);
        Debug.Log($"OnShadowTriggerExit: " + other.name);
        isTriggered = false;
        _command.ExecuteCommand(new MoveCommand(input));
    }

    private void Start()
    {
        _command.ExecuteCommand(new IdleCommand());
    }

    private void FixedUpdate()
    {
        if (isTriggered && !player.isDead)
        {
            if (Time.time > nextAttackTime)
            {
                PerformAttack();
            }
        }
        else if (ifPlayerIsNear())
        {
            ChasePlayer();
        }
        else Patrol();
    }

    public override void Hurt(float damageAmount)
    {
        base.Hurt(damageAmount);
        _command.ExecuteCommand(new HurtCommand());
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

    public void PerformAttack()
    {
        _command.ExecuteCommand(new AttackCommand("A"));
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
