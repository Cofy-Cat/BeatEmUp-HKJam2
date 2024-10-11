using UnityEngine;

public enum CharacterStateId
{
    Idle,
    Move,
}

public abstract class CharacterState: MonoState<CharacterStateId, CharacterStateMachine> {}

public class CharacterStateMachine: MonoStateMachine<CharacterStateId, CharacterStateMachine>
{
    public Rigidbody2D Rigidbody;
}