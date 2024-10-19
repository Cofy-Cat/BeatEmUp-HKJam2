using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BossEnemyController : Controller
{
    private Controller player;
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] float comboCooldown = 2f;
    [SerializeField] float maxComboAttackGap = 1.5f;
    [SerializeField] float attackRange = 10f;
    private float nextAttackTime;
    private float nextComboTime;
    [SerializeField] float hurtDuration = 1.5f;
    string[] attackPool = { "A", "B", "C" };
    string comboSequence = "";

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player").GetComponent<Controller>();
        Assert.NotNull(player);
        nextAttackTime = Time.time + attackCooldown;
        nextComboTime = Time.time + comboCooldown;

        _command.RegisterPattern(new ComboAttackPattern(new[] { "A" }, 0, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new[] { "A", "A" }, 1, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new[] { "A", "A", "A" }, 2, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new[] { "B" }, 0, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new[] { "B", "B" }, 1, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new[] { "B", "B", "B" }, 2, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new[] { "C" }, 0, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new[] { "C", "C" }, 1, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new[] { "D" }, 0, maxComboAttackGap));
    }

    protected override void OnShadowTriggerEnter(Collider2D other)
    {
        base.OnShadowTriggerEnter(other);
        if (!player.isDead && Time.time > nextComboTime && Time.time > nextAttackTime)
        {
            comboSequence = "D";
            PerformAttack("D");
            nextComboTime = Time.time + comboCooldown;
        }
        Debug.Log($"OnShadowTriggerEnter: " + other.name);
    }

    private void Start()
    {
        _command.ExecuteCommand(new IdleCommand());
    }

    private void FixedUpdate()
    {
        // Pattern: Attack if combo is ready and player is not dead, else just idle
        if (!player.isDead && Time.time > nextComboTime && Time.time > nextAttackTime && ifPlayerInRange())
            PrepareAttack();
    }

    private bool ifPlayerInRange()
    {
        return Vector3.Distance(player.transform.position, transform.position) < attackRange;
    }

    private void PrepareAttack()
    {
        if (comboSequence.Contains("A") && comboSequence.Length <= 3) PerformAttack("A");
        else if (comboSequence.Contains("B") && comboSequence.Length <= 3) PerformAttack("B");
        else if (comboSequence.Contains("C") && comboSequence.Length <= 2) PerformAttack("C");
        else comboSequence = DrawAttack();
    }

    private string DrawAttack()
    {
        string avoid = "";
        List<string> newPool = new List<string>();

        if (!comboSequence.Equals(""))
        {
            nextComboTime = Time.time + comboCooldown;
            avoid = comboSequence.Substring(0, 1);
        }

        Debug.Log("Avoid: " + avoid);

        foreach (var attack in attackPool) if (!avoid.Equals(attack)) newPool.Add(attack);

        return newPool[UnityEngine.Random.Range(0, newPool.Count)];
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
