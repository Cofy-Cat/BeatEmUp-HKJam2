using System;
using cfEngine.Logging;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: Controller
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private float maxDashClickGap = 0.3f;
    [SerializeField] private float maxComboAttackGap = 0.2f;
    
    private Vector2 _lastMoveInput = Vector2.zero;
    public Vector2 LastMoveInput => _lastMoveInput;

    protected override void OnEnable()
    {
        base.OnEnable();
        _input.onActionTriggered += OnActionTriggered;
        _sm.onAfterStateChange += OnStateChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _input.onActionTriggered -= OnActionTriggered;
        _sm.onAfterStateChange -= OnStateChanged;
    }

    private void Start()
    {
        _command.RegisterPattern(new DashPattern(maxDashClickGap));
        _command.RegisterPattern(new ComboAttackPattern(new [] { "A", "A"}, 0, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new [] {"A", "A", "A"}, 1, maxComboAttackGap));
        
        _sm.GoToState(CharacterStateId.Idle);
    }
    
    private void OnStateChanged(MonoStateMachine<CharacterStateId, CharacterStateMachine>.StateChangeRecord stateChange)
    {
        if (stateChange.LastState != null && stateChange.LastState.Id == CharacterStateId.AttackEnd)
        {
            if (_lastMoveInput != Vector2.zero)
            {
                _command.ExecuteCommand(new MoveCommand(_lastMoveInput));
            }
        }
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move":
                OnMove(context);
                break;
            case "Attack":
                OnAttack(context);
                break;
            case "HeavyAttack":
                OnHeavyAttack();
                break;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _lastMoveInput = context.ReadValue<Vector2>();
        
        if (_lastMoveInput != Vector2.zero)
        {
            _command.ExecuteCommand(new MoveCommand(_lastMoveInput));
        }
        else
        {
            _command.ExecuteCommand(new IdleCommand());
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isCarrying)
            {
                _command.ExecuteCommand(new AttackCommand("A"));
            }
            else
            {
                _command.ExecuteCommand(new ThrowCommand());
            }
        }
    }
    
    public override void OnHeavyAttack()
    {
        base.OnHeavyAttack();
    }
    
    protected override void OnShadowTriggerEnter(Collider2D other)
    {
        base.OnShadowTriggerEnter(other);
        var throwable = other.GetComponentInParent<Throwable>();

        if (throwable != null)
        {
            _command.ExecuteCommand(new CarryCommand(throwable));
        }
    }

    public override void Attack()
    {
        base.Attack();

        var triggers = _shadow.Triggers;
        
        for (var i = 0; i < triggers.Count; i++)
        {
            var enemy = triggers[i].GetComponentInParent<EnemyController>();
            if (enemy == null)
                continue;

            if (Math.Sign(LastFaceDirection) == Math.Sign(enemy.transform.position.x - transform.position.x))
            {
                enemy.Hurt(attackDamage);
            }
        }
    }
}