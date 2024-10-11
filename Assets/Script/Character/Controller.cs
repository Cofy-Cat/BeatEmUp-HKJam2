using UnityEngine;

public partial class AnimationName
{
    //Make sure your animation name follow this
    public const string IdleRight = "IdleRight";
    public const string IdleLeft = "IdleLeft";
}

public abstract class Controller : MonoBehaviour
{
    [SerializeField] protected Collider2DComponent _shadow;
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected SpriteAnimation _anim;
    [SerializeField] protected CharacterStateMachine _sm;
    [SerializeField] protected ActionCommandController _command;

    protected virtual void Awake()
    {
        _sm.Rigidbody = _rb;
        _sm.Animation = _anim;
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
