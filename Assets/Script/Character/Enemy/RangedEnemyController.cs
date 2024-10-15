using System;
using UnityEngine;

public class RangedEnemyController : Controller
{
    [SerializeField] private GameObject player;
    private Vector2 input;
    [SerializeField] float shootRange = 10f;
    [SerializeField] float attackCooldown = 3f;
    private float nextAttackTime;
    [SerializeField] float hurtDuration = 2f;
    private float nextRecoverTime;
    [SerializeField] float changeInputTime = 4f;
    [SerializeField] GameObject bulletPrefab;
    private float nextChangeInputTime;
    private float dangerRange;
    protected override void Awake()
    {
        base.Awake();
        input = Vector2.zero;
        nextAttackTime = 0f;
        nextRecoverTime = 0f;
        nextChangeInputTime = 0f;
        dangerRange = shootRange / 2;
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
        input = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        // Tries to dash away from player
    }

    private void OnShadowTriggerExit(Collider2D collider)
    {
        Debug.Log($"OnShadowTriggerExit: " + collider.name);
        _command.ExecuteCommand(new MoveCommand(input));
    }

    private void Start()
    {
        _command.ExecuteCommand(new IdleCommand());
    }

    private void FixedUpdate()
    {
        // Pattern: Attack whenever possible -> if player is in danger zone, flee -> if player is near, get into position if can attack -> 
        // if cannot attack or if player is not near, pretend to be dumb and wander
        if (CanPerformAction())
        {
            if (CanAttack() && InPosition()) PerformAttack();
            else if (IsInDanger()) Flee();
            else if (ifPlayerIsNear() && !InPosition() && CanAttack()) GetInPosition();
            else Wander();
        }
    }

    private bool CanPerformAction()
    {
        return Time.time > nextRecoverTime;
    }
    private bool IsInDanger()
    {
        return Vector3.Distance(player.transform.position, transform.position) < dangerRange;
    }

    private bool InPosition()
    {
        return Math.Abs(player.transform.position.y - transform.position.y) < 0.1f;
    }

    private bool CanAttack()
    {
        return Time.time > nextAttackTime;
    }
    private bool ifPlayerIsNear()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < shootRange) return true;
        else return false;
    }
    private void Flee()
    {
        input = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        input.Normalize();
        _command.ExecuteCommand(new MoveCommand(-input));
    }
    private void GetInPosition()
    {
        input.y = player.transform.position.y - transform.position.y;
        input.Normalize();
        _command.ExecuteCommand(new MoveCommand(input));
    }

    private void Wander()
    {
        if (Time.time > nextChangeInputTime)
        {
            input = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            input.Normalize();
            nextChangeInputTime = Time.time + changeInputTime;
        }
        _command.ExecuteCommand(new MoveCommand(input));
    }
    public override void Hurt(float damageAmount)
    {
        base.Hurt(damageAmount);
        _command.ExecuteCommand(new HurtCommand());
        nextRecoverTime = Time.time + hurtDuration;
    }

    public void PerformAttack()
    {
        _command.ExecuteCommand(new AttackCommand());
        nextAttackTime = Time.time + attackCooldown;
    }

    public override void Attack()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.direction = Math.Sign(player.transform.position.x - transform.position.x);
        bullet.enabled = true;
    }
}
