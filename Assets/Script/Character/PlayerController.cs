using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: Controller
{
    [SerializeField] private PlayerInput _input;

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

    private void Update()
    {
        _anim.playSpriteSwapAnimation(AnimationName.IdleRight, true);
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
    }
}