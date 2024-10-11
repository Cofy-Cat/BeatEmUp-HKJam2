using System;
using UnityEngine;

public partial class AnimationName
{
    //Make sure your animation name follow this
    public const string IdleRight = nameof(IdleRight);
    public const string IdleLeft = nameof(IdleLeft);
    public const string WalkRight = nameof(WalkRight);
    public const string WalkLeft = nameof(WalkLeft);
}

public abstract class Controller : MonoBehaviour
{
    [SerializeField] protected Collider2DComponent _shadow;
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected SpriteAnimation _anim;
    [SerializeField] protected CharacterStateMachine _sm;
    [SerializeField] protected ActionCommandController _command;

    [Header("Stat")] 
    public Vector2 moveSpeed = Vector2.one;
    
    private Vector2 _lastFaceDirection = Vector2.right;
    public Vector2 LastFaceDirection => _lastFaceDirection;

    public SpriteAnimation Animation => _anim;
    public Rigidbody2D Rigidbody => _rb;

    protected virtual void Awake()
    {
        _sm.Controller = this;
        _command.StateMachine = _sm;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _rb.linearVelocity = velocity;

        if (velocity != Vector2.zero)
        {
            _lastFaceDirection = velocity / (velocity.x > velocity.y ? velocity.x : velocity.y);
        }
    }
}
