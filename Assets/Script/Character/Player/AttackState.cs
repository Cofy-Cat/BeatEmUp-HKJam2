using UnityEngine;

public class AttackState: CharacterState
{
    public override CharacterStateId[] stateBlacklist => new[] { CharacterStateId.Move, CharacterStateId.Idle, CharacterStateId.Attack };
    public override CharacterStateId Id => CharacterStateId.Attack;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        sm.Controller.SetVelocity(Vector2.zero);
        string animationName =
            AnimationName.GetDirectional(AnimationName.Attack, sm.Controller.LastFaceDirection);

        sm.Controller.Animation.playSpriteSwapAnimation(animationName, onAnimationEnd: () =>
        {
            sm.Controller.Attack();
            sm.GoToState(CharacterStateId.AttackEnd);
        });
    }
}