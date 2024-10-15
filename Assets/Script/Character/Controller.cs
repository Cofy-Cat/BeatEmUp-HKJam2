using System;
using JetBrains.Annotations;
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

    public static string GetComboDirectional(string animationName, int comboCount, float horizontalDirection)
    {
        if (comboCount == 1)
        {
            return GetDirectional(animationName, horizontalDirection);
        }

        return GetDirectional($"{animationName}{comboCount}", horizontalDirection);
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
}
