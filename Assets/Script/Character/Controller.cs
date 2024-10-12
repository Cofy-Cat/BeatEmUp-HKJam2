using System;
using UnityEngine;

public partial class AnimationName
{
    //Make sure your animation name follow this
    public const string IdleRight = nameof(IdleRight);
    public const string IdleLeft = nameof(IdleLeft);
    public const string WalkRight = nameof(WalkRight);
    public const string WalkLeft = nameof(WalkLeft);
    public const string HurtLeft = nameof(HurtLeft);
    public const string HurtRight = nameof(HurtRight);
    public const string DashLeft = nameof(DashLeft);
    public const string DashRight = nameof(DashRight);
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

    public Vector2 dashSpeed = Vector2.one;

    private float _lastHorizontalDirection = 0f;
    public float LastHorizontalDirection => _lastHorizontalDirection;

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

        if (!Mathf.Approximately(velocity.x, 0f))
        {
            _lastHorizontalDirection = Mathf.Sign(velocity.x) * velocity.x / velocity.x;
        }
    }
}
