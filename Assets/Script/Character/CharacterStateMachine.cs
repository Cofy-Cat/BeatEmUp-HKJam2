public enum CharacterStateId
{
    Idle,
    Move,
    Hurt,
    Dash,
    Attack
}

public abstract class CharacterState : MonoState<CharacterStateId, CharacterStateMachine> { }

public class CharacterStateMachine : MonoStateMachine<CharacterStateId, CharacterStateMachine>
{
    public Controller Controller;
}