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
    protected override void OnShadowTriggerEnter(Collider2D other)
    {
        base.OnShadowTriggerEnter(other);
        Debug.Log($"OnShadowTriggerEnter: " + other.name);
        setInput();
        // Tries to dash away from player
    }

    protected override void OnShadowTriggerExit(Collider2D other)
    {
        base.OnShadowTriggerExit(other);
        Debug.Log($"OnShadowTriggerExit: " + other.name);
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
        setInput(new Vector2(-(player.transform.position.x - transform.position.x), -(player.transform.position.y - transform.position.y)));
        _command.ExecuteCommand(new MoveCommand(input));
    }
    private void GetInPosition()
    {
        setInput(new Vector2(input.x, player.transform.position.y - transform.position.y));
        _command.ExecuteCommand(new MoveCommand(input));
    }

    private void Wander()
    {
        setInput();
        _command.ExecuteCommand(new MoveCommand(input));
    }
    public override void Hurt(float damageAmount)
    {
        base.Hurt(damageAmount);
        nextRecoverTime = Time.time + hurtDuration;
    }

    public void PerformAttack()
    {
        // Turn to face player
        if (player.transform.position.x < transform.position.x) _command.ExecuteCommand(new MoveCommand(new Vector2(-1, 0)));
        _command.ExecuteCommand(new AttackCommand("A"));
        nextAttackTime = Time.time + attackCooldown;
    }

    public void setInput(Vector2 input)
    {
        if (Time.time > nextChangeInputTime)
        {
            this.input = input;
            this.input.Normalize();
            nextChangeInputTime = Time.time + changeInputTime;
        }
    }

    public void setInput()
    {
        if (Time.time > nextChangeInputTime)
        {
            input = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            input.Normalize();
            nextChangeInputTime = Time.time + changeInputTime;
        }
    }

    public override bool Attack(AttackConfig config)
    {
        Vector3 bulletPosition = new Vector3(transform.position.x, transform.position.y + 1.75f, transform.position.z);
        Bullet bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity).GetComponent<Bullet>();
        // Turn bulle.direction to face player
        bullet.enabled = true;
        bullet.gameObject.SetActive(true);
        bullet.direction = player.transform.position.x - transform.position.x;
        return false;
    }
}
