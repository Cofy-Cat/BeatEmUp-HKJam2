using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: Controller
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private float maxDashClickGap = 0.3f;
    [SerializeField] private float maxComboAttackGap = 0.2f;
    
    [SerializeField] private Vector2 _lastMoveInput = Vector2.zero;
    public Vector2 LastMoveInput => _lastMoveInput;

    protected void OnEnable()
    {
        _input.onActionTriggered += OnActionTriggered;
        _sm.onAfterStateChange += OnStateChanged;
    }

    protected void OnDisable()
    {
        _input.onActionTriggered -= OnActionTriggered;
        _sm.onAfterStateChange -= OnStateChanged;
    }

    private void Start()
    {
        _command.RegisterPattern(new DashPattern(maxDashClickGap));
        _command.RegisterPattern(new RepeatAttackPattern(2, maxComboAttackGap));
        
        _sm.GoToState(CharacterStateId.Idle);
    }
    
    private void OnStateChanged(MonoStateMachine<CharacterStateId, CharacterStateMachine>.StateChangeRecord stateChange)
    {
        if (stateChange.LastState != null && stateChange.LastState.Id == CharacterStateId.Attack)
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
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _lastMoveInput = context.ReadValue<Vector2>();
        
        if (context.performed)
        {
            _command.ExecuteCommand(new MoveCommand(_lastMoveInput));
        }
        else if(context.canceled)
        {
            _command.ExecuteCommand(new IdleCommand());
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _command.ExecuteCommand(new AttackCommand());
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

            enemy.Hurt();
        }
    }
}