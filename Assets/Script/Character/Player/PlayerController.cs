using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: Controller
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private float maxDashClickGap = 0.3f;
    [SerializeField] private float maxComboAttackGap = 0.2f;
    public AudioClip moveClip;
    
    private Vector2 _lastMoveInput = Vector2.zero;
    public Vector2 LastMoveInput => _lastMoveInput;

    public event Action<Controller> onAttack;

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
        _command.RegisterPattern(new ComboAttackPattern(new [] { "B" }, 0, maxComboAttackGap));
        _command.RegisterPattern(new ComboAttackPattern(new [] { "B", "B" }, 1, maxComboAttackGap));
        
        _sm.GoToState(CharacterStateId.Idle);
    }
    
    private void OnStateChanged(MonoStateMachine<CharacterStateId, CharacterStateMachine>.StateChangeRecord stateChange)
    {
        if (stateChange.LastState == CharacterStateId.AttackEnd)
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
                OnHeavyAttack(context);
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
    
    public override void OnHeavyAttack(InputAction.CallbackContext context)
    {
        base.OnHeavyAttack(context);

        if (context.performed)
        {
            if (!isCarrying)
            {
                _command.ExecuteCommand(new AttackCommand("B"));
            }
            else
            {
                _command.ExecuteCommand(new ThrowCommand());
            }
        }
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

    public override bool Attack(AttackConfig config)
    {
        var triggers = _shadow.Triggers;
        var targetHitCommand = config?.targetHitAction.GetCommand();
        
        for (var i = 0; i < triggers.Count; i++)
        {
            var enemy = triggers[i].GetComponentInParent<Controller>();
            if(enemy == null || !enemy.gameObject.tag.Equals("Enemy")) continue;

            if (Math.Sign(LastFaceDirection) == Math.Sign(enemy.transform.position.x - transform.position.x))
            {
                enemy.Hurt(attackDamage);
                if (targetHitCommand != null)
                {
                    enemy.Command.ExecuteCommand(targetHitCommand);
                }
                onAttack?.Invoke(enemy);
                return true;
            }
        }

        return false;
    }
}