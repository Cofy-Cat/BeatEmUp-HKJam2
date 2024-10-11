using UnityEngine;

public enum CharacterStateId
{
    Idle,
    Move,
}

public class CharacterStateMachine: MonoStateMachine<CharacterStateId, CharacterStateMachine>
{
    public Rigidbody2D Rigidbody;
}