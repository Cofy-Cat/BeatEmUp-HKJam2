using cfEngine.Util;

public class ThrowEndState: CharacterState
{
    public override CharacterStateId Id => CharacterStateId.ThrowEnd;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        var animationName = AnimationName.GetDirectional(AnimationName.ThrowEnd, sm.Controller.LastFaceDirection);
        
        sm.Controller.Animation.Play(animationName, onAnimationEnd: () =>
        {
            if (sm.CurrentStateId == Id)
            {
                sm.Controller.Command.ExecuteCommand(new IdleCommand());
            }
        });
    }
}