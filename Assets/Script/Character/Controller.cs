using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public const string Death = nameof(Death);
    public const string DashAttack = nameof(DashAttack);

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

[Serializable]
public class HealthRecord
{
    public float current;
    public float max;
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
    private Animation shadowAnimation;
    

    [Header("Stat")]
    public Vector2 moveSpeed = Vector2.one;

    public Vector2 dashSpeed = Vector2.one;

    private float _lastFaceDirection = 0f;
    public float LastFaceDirection => _lastFaceDirection;

    private Vector2 _lastMoveDirection = Vector2.zero;
    public Vector2 LastMoveDirection => _lastMoveDirection;

    [SerializeField] private HealthRecord _health;
    [SerializeField] protected float attackDamage = 10f;
    [SerializeField] protected float attackKnockbackForce = 0.5f;
    [SerializeField] protected Vector2 throwForce = new Vector2(5, 5);
    [SerializeField] protected AttackConfigSO _attackConfig;

    private Throwable attachedThrowable;
    public bool isCarrying => attachedThrowable != null;

    public event Action<HealthRecord> onHealthChange;
    public event Action onDead;

    #region getter

    public SpriteAnimation Animation => _anim;
    public Rigidbody2D Rigidbody => _rb;
    public ActionCommandController Command => _command;
    public Transform MainCharacter => _mainCharacter;
    public HealthRecord Health => _health;
    public AttackConfigSO AttackConfig => _attackConfig;

    public bool isDead => _health.current <= 0;

    #endregion

    protected virtual void Awake()
    {
        _sm.Controller = this;
        _command.StateMachine = _sm;
        shadowAnimation = _shadow.GetComponentInChildren<Animation>();
    }

    private void Start()
    {
        StartCoroutine(BodyValidation());
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

    private IEnumerator BodyValidation()
    {
        var checkPeriod = new WaitForSeconds(1f);
        while (!isDead)
        {
            yield return checkPeriod;

            var bodyTransform = _mainCharacter.transform;
            if (bodyTransform.localPosition.y < 0)
            {
                bodyTransform.localPosition = new Vector2(bodyTransform.localPosition.x, 0);
            }
        }
    }

    protected virtual void OnShadowTriggerEnter(Collider2D other)
    {
        
    }
    
    protected virtual void OnShadowTriggerExit(Collider2D other)
    {
        
    }

    public void SetHealthValue(HealthRecord record)
    {
        if (record == null)
        {
            return;
        }
        _health.current = record.current;
        _health.max = record.max;
        onHealthChange?.Invoke(_health);
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
        
        foreach (AnimationState state in shadowAnimation)
        {
            state.speed = velocity.magnitude / moveSpeed.magnitude;
        }
    }

    public abstract bool Attack(AttackConfig config);

    public virtual void Hurt(float damageAmount)
    {
        if(isDead) return;
        var nextHealth = _health.current - damageAmount;

        if (nextHealth <= 0)
        {
            _health.current = 0;
            onDead?.Invoke();
            _sm.GoToState(CharacterStateId.Dead);
        }
        else
        {
            _health.current = nextHealth;
            _command.ExecuteCommand(new HurtCommand());
        }
        
        onHealthChange?.Invoke(_health);
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

    public virtual void OnHeavyAttack(InputAction.CallbackContext context) {}
}
