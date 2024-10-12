using UnityEngine;

public class IdleState: CharacterState
{
    public override CharacterStateId Id => CharacterStateId.Idle;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        string animationName = AnimationName.GetDirectional(AnimationName.Idle, sm.Controller.LastFaceDirection);

        sm.Controller.Animation.playSpriteSwapAnimation(animationName, true);
        sm.Controller.Rigidbody.linearVelocity = Vector2.zero;
    }
}