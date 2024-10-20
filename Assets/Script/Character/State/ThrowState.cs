using System.Collections.Generic;

public class ThrowState: CharacterState
{
    public override HashSet<CharacterStateId> Whitelist { get; } = new() { CharacterStateId.ThrowEnd };
    public override CharacterStateId Id => CharacterStateId.Throw;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        var animationName = AnimationName.GetDirectional(AnimationName.Throw, sm.Controller.LastFaceDirection);
        sm.Controller.Animation.Play(animationName, onAnimationEnd: () =>
        {
            sm.Controller.Throw();
            sm.GoToState(CharacterStateId.ThrowEnd);
        });
    }
}