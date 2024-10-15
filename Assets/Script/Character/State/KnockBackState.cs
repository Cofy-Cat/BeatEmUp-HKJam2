using NUnit.Framework;
using UnityEngine;

public class KnockBackState: CharacterState
{
    public class Param : StateParam
    {
        public float knockbackDuration;
        public int Direction;
        public float Force;
    }
    
    public override CharacterStateId[] stateBlacklist => new[]
        { CharacterStateId.Attack, CharacterStateId.Dash, CharacterStateId.Move };
    public override CharacterStateId Id => CharacterStateId.KnockBack;

    private float startKnockTime = float.MaxValue;
    private CharacterStateMachine sm;
    private Param p;

    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        p = param as Param;
        this.sm = sm;
        Assert.IsNotNull(p);

        startKnockTime = Time.time;
        sm.Controller.Rigidbody.linearVelocityX = p.Direction * p.Force;
    }

    public override void _Update()
    {
        base._Update();

        if (Time.time - startKnockTime >= p.knockbackDuration)
        {
            sm.Controller.Rigidbody.linearVelocityX = 0;
            sm.Controller.Command.ExecuteCommand(new IdleCommand());
            return;
        }
    }
}