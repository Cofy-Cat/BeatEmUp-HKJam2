public class MoveState: MonoState<CharacterStateId, CharacterStateMachine>
{
    public override CharacterStateId Id => CharacterStateId.Move;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
    }
}