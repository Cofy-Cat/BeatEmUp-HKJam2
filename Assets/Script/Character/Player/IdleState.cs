public class IdleState: MonoState<CharacterStateId, CharacterStateMachine>
{
    public override CharacterStateId Id => CharacterStateId.Idle;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
    }
}