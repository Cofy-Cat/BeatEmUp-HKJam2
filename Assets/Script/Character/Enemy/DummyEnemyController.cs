using UnityEngine;
using System;

public class DummyEnemyController : Controller
{
    private Vector2 input;
    [SerializeField] float hurtDuration = 3f;
    private float nextRecoverTime;
    [SerializeField] float changeActionTime = 6f;
    private float nextChangeActionTime;
    private string currrentAction = "Idle";
    private string[] actions = { "Idle", "Wander" };
    protected override void Awake()
    {
        base.Awake();
        input = Vector2.zero;
        nextRecoverTime = 0f;
        nextChangeActionTime = 0f;
    }
    private void FixedUpdate()
    {
        // Pattern: Attack whenever possible -> if player is in danger zone, flee -> if player is near, get into position if can attack -> 
        // if cannot attack or if player is not near, pretend to be dumb and wander
        if (CanPerformAction())
        {
            if (CanDrawNextAction()) DrawNextAction();
            else if (currrentAction == "Wander") _command.ExecuteCommand(new MoveCommand(input));
        }
    }

    private bool CanPerformAction()
    {
        return Time.time > nextRecoverTime;
    }
    private bool CanDrawNextAction()
    {
        return Time.time > nextChangeActionTime;
    }
    private void DrawNextAction()
    {
        currrentAction = actions[UnityEngine.Random.Range(0, actions.Length)];
        nextChangeActionTime = Time.time + changeActionTime;
        if (currrentAction == "Idle") _command.ExecuteCommand(new IdleCommand());
        else if (currrentAction == "Wander")
        {
            setInput();
            _command.ExecuteCommand(new MoveCommand(input));
        }
    }

    public override bool Attack(AttackConfig config)
    {
        return false;
    }

    public override void Hurt(float damageAmount)
    {
        base.Hurt(damageAmount);
        _command.ExecuteCommand(new HurtCommand());
        nextRecoverTime = Time.time + hurtDuration;
    }
    public void setInput()
    {
        input = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        input.Normalize();
    }
}
