using System;
using cfEngine.Logging;
using UnityEngine;
using UnityEngine.Rendering;

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
    protected override void Awake()
    {
        base.Awake();
        input = Vector2.zero;
        direction = Vector3.zero;
        patrolStartPos = transform.position;
        patrolEndPos = transform.position + new Vector3(patrolRange, 0, 0);
        nextAttackTime = Time.time;
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
    }

    private void Start()
    {
        _command.ExecuteCommand(new IdleCommand());
    }

    private void FixedUpdate()
    {
        if (isTriggered) Attack();
        else if (ifPlayerIsNear()) ChasePlayer();
        else Patrol();
    }

    public void Hurt()
    {
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
    }

    private void Patrol()
    {
        if (transform.position.x <= patrolStartPos.x)
        {
            input = new Vector2(1, 0);
            _command.ExecuteCommand(new MoveCommand(input));
        }
        else if (transform.position.x > patrolEndPos.x)
        {
            input = new Vector2(-1, 0);
            _command.ExecuteCommand(new MoveCommand(input));
        }
    }

    private void Attack() {
        if (Time.time > nextAttackTime) {
            _command.ExecuteCommand(new AttackCommand());
            nextAttackTime = Time.time + attackCooldown;
        }
    }
}
