using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: Controller
{
    [SerializeField] private PlayerInput _input;
    
    private void Update()
    {
        _anim.playSpriteSwapAnimation(AnimationName.IdleRight, true);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
    }
}