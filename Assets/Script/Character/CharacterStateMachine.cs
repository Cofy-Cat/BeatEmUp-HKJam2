using UnityEngine;

public enum CharacterStateId
{
    Idle,
    Move,
    Hurt,
    Dash
}

public abstract class CharacterState : MonoState<CharacterStateId, CharacterStateMachine> { }

public class CharacterStateMachine : MonoStateMachine<CharacterStateId, CharacterStateMachine>
{
    public Controller Controller;
}