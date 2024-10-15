using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class KnockBackState: CharacterState
{
    [SerializeField] private float knockbackDuration = 0.3f;
    
    public class Param : StateParam
    {
        public int Direction;
        public float Force;
    }
    
    public override CharacterStateId[] stateBlacklist => new[]
        { CharacterStateId.Attack, CharacterStateId.Dash, CharacterStateId.Move };
    public override CharacterStateId Id => CharacterStateId.KnockBack;

    private float startKnockTime = float.MaxValue;
    private CharacterStateMachine sm;

    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        var p = param as Param;
        this.sm = sm;
        Assert.IsNotNull(p);

        startKnockTime = Time.time;
        sm.Controller.Rigidbody.linearVelocityX = p.Direction * p.Force;
    }

    public override void _Update()
    {
        base._Update();

        if (Time.time - startKnockTime >= knockbackDuration)
        {
            sm.Controller.Rigidbody.linearVelocityX = 0;
            sm.Controller.Command.ExecuteCommand(new IdleCommand());
            return;
        }
    }
}