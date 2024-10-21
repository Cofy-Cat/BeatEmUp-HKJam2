using cfUnityEngine.Util;

public enum CharacterStateId
{
    Idle,
    Move,
    Hurt,
    Dash,
    Attack,
    AttackEnd,
    KnockBack,
    Carry,
    Throw,
    ThrowEnd,
    Dead
}

public abstract class CharacterState : MonoState<CharacterStateId, CharacterState, CharacterStateMachine> { }

public class CharacterStateMachine : MonoStateMachine<CharacterStateId, CharacterState, CharacterStateMachine>
{
    public Controller Controller;
}