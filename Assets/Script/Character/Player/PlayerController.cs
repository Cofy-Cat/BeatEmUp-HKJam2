using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: Controller
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private float maxDashClickGap = 0.3f;

    protected void OnEnable()
    {
        _input.onActionTriggered += OnActionTriggered;
    }

    protected void OnDisable()
    {
        _input.onActionTriggered -= OnActionTriggered;
    }

    private void Start()
    {
        _command.RegisterPattern(new DashPattern(maxDashClickGap));
        
        _sm.GoToState(CharacterStateId.Idle);
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
        if (context.performed)
        {
            var input = context.ReadValue<Vector2>();
            _command.ExecuteCommand(new MoveCommand(input));
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
}