using System.Collections.Generic;
using cfEngine.Util;
using UnityEngine;

public class IdleState: CharacterState
{
    public override HashSet<CharacterStateId> Whitelist { get; } = 
        new() { 
            CharacterStateId.Attack,
            CharacterStateId.Move,
            CharacterStateId.Dash,
            CharacterStateId.Hurt,
            CharacterStateId.Dead 
        };
    public override CharacterStateId Id => CharacterStateId.Idle;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        string animationName;
        if (!sm.Controller.isCarrying)
        {
            animationName = AnimationName.GetDirectional(AnimationName.Idle, sm.Controller.LastFaceDirection);
        }
        else
        {
            animationName = AnimationName.GetDirectional(AnimationName.Carry, sm.Controller.LastFaceDirection);
        }

        sm.Controller.Animation.Play(animationName, true);
        sm.Controller.Rigidbody.linearVelocity = Vector2.zero;
    }
}