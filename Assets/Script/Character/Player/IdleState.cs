using UnityEngine;

public class IdleState: CharacterState
{
    public override CharacterStateId Id => CharacterStateId.Idle;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        sm.Animation.playSpriteSwapAnimation(AnimationName.IdleRight, true);
        sm.Rigidbody.linearVelocity = Vector2.zero;
    }
}