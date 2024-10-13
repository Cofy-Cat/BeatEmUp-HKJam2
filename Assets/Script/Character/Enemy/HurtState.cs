using System;
using UnityEngine;
using UnityEngine.Assertions;

public class HurtState : CharacterState
{
    public override CharacterStateId Id => CharacterStateId.Hurt;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        string animationName = String.Empty;
        if (sm.Controller.LastFaceDirection >= 0)
        {
            animationName = AnimationName.HurtRight;
            sm.Controller.SetVelocity(new Vector2(-0.5f, 0));
        }
        else if (sm.Controller.LastFaceDirection < 0)
        {
            animationName = AnimationName.HurtLeft;
            sm.Controller.SetVelocity(new Vector2(0.5f, 0));
        }

        sm.Controller.Animation.playSpriteSwapAnimation(animationName, true);
    }
}