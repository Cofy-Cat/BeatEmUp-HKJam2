using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: Controller
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private ActionCommandController _command;
    [SerializeField] private PlayerStateMachine _sm;

    private void Awake()
    {
        _sm.Rigidbody = _rb;
        _command.StateMachine = _sm;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _input.onActionTriggered += OnActionTriggered;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _input.onActionTriggered -= OnActionTriggered;
    }

    private void Start()
    {
        _command.EnqueueCommand(new IdleCommand());
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move":
                OnMove(context);
                break;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        _command.EnqueueCommand(new MoveCommand(input));
    }
}