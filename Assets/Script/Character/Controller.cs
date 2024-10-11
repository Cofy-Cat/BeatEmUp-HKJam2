using UnityEngine;

public partial class AnimationName
{
    //Make sure your animation name follow this
    public const string IdleRight = nameof(IdleRight);
    public const string IdleLeft = nameof(IdleLeft);
    public const string WalkRight = nameof(WalkRight);
    public const string WalkLeft = nameof(WalkLeft);
}

public abstract class Controller : MonoBehaviour
{
    [SerializeField] protected Collider2DComponent _shadow;
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected SpriteAnimation _anim;
    [SerializeField] protected CharacterStateMachine _sm;
    [SerializeField] protected ActionCommandController _command;

    public SpriteAnimation Animation => _anim;
    public Rigidbody2D Rigidbody => _rb;

    protected virtual void Awake()
    {
        _sm.Controller = this;
        _command.StateMachine = _sm;
    }

    protected virtual void OnEnable()
    {
        _shadow.triggerEnter += ShadowOntriggerEnter;
    }

    protected virtual void OnDisable()
    {
        _shadow.triggerEnter -= ShadowOntriggerEnter;
    }

    private void ShadowOntriggerEnter(Collider2D obj)
    {
    }
}
