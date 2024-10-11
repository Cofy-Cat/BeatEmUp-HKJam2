using UnityEngine;

public class PlayerController: Controller
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteAnimation _anim;

    private void Update()
    {
        _anim.playSpriteSwapAnimation(AnimationName.IdleRight, true);
    }
}