public class MoveState: CharacterState
{
    public override CharacterStateId Id => CharacterStateId.Move;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        sm.Animation.playSpriteSwapAnimation(AnimationName.WalkRight, true);
    }
}