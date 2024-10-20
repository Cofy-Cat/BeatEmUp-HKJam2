public class AttackEndState: CharacterState
{
    public override CharacterStateId Id => CharacterStateId.AttackEnd;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        var p = (AttackState.Param)param;
        
        string animationName =
            AnimationName.GetComboDirectional(AnimationName.AttackEnd, p.Combo, sm.Controller.LastFaceDirection);
        
        sm.Controller.Animation.Play(animationName, onAnimationEnd: () =>
        {
            if (sm.CurrentStateId != CharacterStateId.AttackEnd) return;
            
            sm.Controller.Command.ExecuteCommand(new IdleCommand());
        });
    }
}