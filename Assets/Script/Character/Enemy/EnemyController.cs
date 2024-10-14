using System;
using UnityEngine;

public class EnemyController : Controller
{
    [SerializeField] private GameObject player;
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
    private float isHurting;
    protected override void Awake()
    {
        base.Awake();
        input = Vector2.zero;
        direction = Vector3.zero;
        patrolStartPos = transform.position;
        patrolEndPos = transform.position + new Vector3(patrolRange, 0, 0);
        nextAttackTime = 0f;
        isHurting = 0f;
    }
    private void OnEnable()
    {
        _shadow.triggerEnter += OnShadowTriggerEnter;
        _shadow.triggerExit += OnShadowTriggerExit;
    }
    private void OnDisable()
    {
        _shadow.triggerEnter -= OnShadowTriggerEnter;
        _shadow.triggerExit -= OnShadowTriggerExit;
    }
    private void OnShadowTriggerEnter(Collider2D collider)
    {
        Debug.Log($"OnShadowTriggerEnter: " + collider.name);
        isTriggered = true;
    }

    private void OnShadowTriggerExit(Collider2D collider)
    {
        Debug.Log($"OnShadowTriggerExit: " + collider.name);
        isTriggered = false;
        if (isHurting > Time.time) { }
        else _command.ExecuteCommand(new MoveCommand(input));
    }

    private void Start()
    {
        _command.ExecuteCommand(new IdleCommand());
    }

    private void FixedUpdate()
    {
        if (isHurting > Time.time) { }
        else if (isTriggered)
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
        isHurting = Time.time + hurtDuration;
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
        _command.ExecuteCommand(new AttackCommand());
        nextAttackTime = Time.time + attackCooldown;
    }

    public override void Attack()
    {
        base.Attack();
        
        var triggers = _shadow.Triggers;
        
        for (var i = 0; i < triggers.Count; i++)
        {
            var player = triggers[i].GetComponentInParent<PlayerController>();
            if (player == null)
                continue;

            if (Math.Sign(LastFaceDirection) == Math.Sign(player.transform.position.x - transform.position.x))
            {
                player.Hurt(attackDamage);
                player.Command.ExecuteCommand(new KnockBackCommand(Math.Sign(LastFaceDirection), attackKnockbackForce));
            }
        }
    }
}
