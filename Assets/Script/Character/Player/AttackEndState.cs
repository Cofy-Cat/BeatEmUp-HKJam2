public class AttackEndState: CharacterState
{
    public override CharacterStateId Id => CharacterStateId.AttackEnd;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        string animationName =
            AnimationName.GetDirectional(AnimationName.AttackEnd, sm.Controller.LastFaceDirection);
        
        sm.Controller.Animation.playSpriteSwapAnimation(animationName, onAnimationEnd: () =>
        {
            if (sm.CurrentState.Id != CharacterStateId.AttackEnd) return;
            
            sm.Controller.Command.ExecuteCommand(new IdleCommand());
        });
    }
}