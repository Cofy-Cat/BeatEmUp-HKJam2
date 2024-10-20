using System;
using System.Collections.Generic;
using UnityEngine;

public class DeadState: CharacterState
{
    public override HashSet<CharacterStateId> Whitelist { get; } = new();
    public override CharacterStateId Id => CharacterStateId.Dead;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        sm.Controller.SetVelocity(Vector2.zero);
        sm.Controller.Animation.Play(AnimationName.GetDirectional(AnimationName.Death, sm.Controller.LastFaceDirection));
    }
}