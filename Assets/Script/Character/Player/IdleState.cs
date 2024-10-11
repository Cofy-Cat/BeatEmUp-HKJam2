using System;
using UnityEngine;
using UnityEngine.Assertions;

public class IdleState: CharacterState
{
    public override CharacterStateId Id => CharacterStateId.Idle;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        string animationName = String.Empty;
        if (sm.Controller.LastFaceDirection.x > 0)
        {
            animationName = AnimationName.IdleRight;
        } else if (sm.Controller.LastFaceDirection.x < 0)
        {
            animationName = AnimationName.IdleLeft;
        }
        
        Assert.AreNotEqual(animationName, String.Empty);

        sm.Controller.Animation.playSpriteSwapAnimation(animationName, true);
        sm.Controller.Rigidbody.linearVelocity = Vector2.zero;
    }
}