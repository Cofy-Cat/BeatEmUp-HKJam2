public class AttackState: CharacterState
{
    public override CharacterStateId Id => CharacterStateId.Attack;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        string animationName =
            AnimationName.GetDirectional(AnimationName.Attack, sm.Controller.LastHorizontalDirection);

        sm.Controller.Animation.playSpriteSwapAnimation(animationName, onAnimationEnd: () =>
        {
            sm.GoToState(CharacterStateId.Idle);
        });
    }
}