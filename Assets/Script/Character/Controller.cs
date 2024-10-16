using System;
using UnityEngine;

public partial class AnimationName
{
    //Make sure your animation name follow this
    public const string Idle = nameof(Idle);
    public const string Walk = nameof(Walk);
    public const string HurtLeft = nameof(HurtLeft);
    public const string HurtRight = nameof(HurtRight);
    public const string Dash = nameof(Dash);
    public const string Attack = nameof(Attack);
    public const string AttackEnd = nameof(AttackEnd);
    public const string Carry = nameof(Carry);
    public const string CarryWalk = nameof(CarryWalk);
    public const string Throw = nameof(Throw);
    public const string ThrowEnd = nameof(ThrowEnd);

    public static string GetDirectional(string animationName, float horizontalDirection)
    {
        if (horizontalDirection >= 0)
        {
            return $"{animationName}Right";
        }
        else
        {
            return $"{animationName}Left";
        }
    }

    public static string GetComboDirectional(string animationName, string[] combo, float horizontalDirection)
    {
        var comboString = combo == null ? string.Empty : string.Join("", combo);
        return GetDirectional($"{animationName}{comboString}", horizontalDirection);
    }
}

public abstract class Controller : MonoBehaviour
{
    [SerializeField] protected Collider2DComponent _shadow;
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected SpriteAnimation _anim;
    [SerializeField] protected CharacterStateMachine _sm;
    [SerializeField] protected ActionCommandController _command;
    [SerializeField] protected Transform _mainCharacter;
    [SerializeField] protected Transform _throwableAttachPoint;
    

    [Header("Stat")]
    public Vector2 moveSpeed = Vector2.one;

    public Vector2 dashSpeed = Vector2.one;

    private float _lastFaceDirection = 0f;
    public float LastFaceDirection => _lastFaceDirection;

    private Vector2 _lastMoveDirection = Vector2.zero;
    public Vector2 LastMoveDirection => _lastMoveDirection;

    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth = 100;
    [SerializeField] protected float attackDamage = 10f;
    [SerializeField] protected float attackKnockbackForce = 0.5f;
    [SerializeField] protected Vector2 throwForce = new Vector2(5, 5);

    private Throwable attachedThrowable;
    public bool isCarrying => attachedThrowable != null;

    public event Action<float> onHealthChange;
    public event Action onDead;

    #region getter

    public SpriteAnimation Animation => _anim;
    public Rigidbody2D Rigidbody => _rb;
    public ActionCommandController Command => _command;
    public Transform MainCharacter => _mainCharacter;

    #endregion

    protected virtual void Awake()
    {
        _sm.Controller = this;
        _command.StateMachine = _sm;
    }

    protected virtual void OnEnable()
    {
        _shadow.triggerEnter += OnShadowTriggerEnter;
        _shadow.triggerExit += OnShadowTriggerExit;
    }

    protected virtual void OnDisable()
    {
        _shadow.triggerEnter -= OnShadowTriggerEnter;
        _shadow.triggerExit -= OnShadowTriggerExit;
    }

    protected virtual void OnShadowTriggerEnter(Collider2D other)
    {
        
    }
    
    protected virtual void OnShadowTriggerExit(Collider2D other)
    {
        
    }

    public void SetVelocity(Vector2 velocity)
    {
        _rb.linearVelocity = velocity;

        if (velocity != Vector2.zero)
        {
            _lastMoveDirection = velocity.normalized;
        }

        if (!Mathf.Approximately(velocity.x, 0f))
        {
            _lastFaceDirection = Mathf.Sign(velocity.x) * velocity.x / velocity.x;
        }
    }

    public virtual void Attack() {}

    public virtual void Hurt(float damageAmount)
    {
        var nextHealth = currentHealth - damageAmount;

        if (nextHealth <= 0)
        {
            currentHealth = 0;
            onDead?.Invoke();
        }
        else
        {
            currentHealth = nextHealth;
        }
        
        onHealthChange?.Invoke(nextHealth);
    }
    
    public void AttachThrowable(Throwable throwable)
    {
        throwable.AttachToTransform(_throwableAttachPoint);
        attachedThrowable = throwable;
    }

    public void Throw()
    {
        attachedThrowable.Throw(new Vector2(throwForce.x * _lastFaceDirection, throwForce.y), transform.position.y);
        attachedThrowable = null;
    }
}
