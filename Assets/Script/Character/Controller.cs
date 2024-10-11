using UnityEngine;

public partial class AnimationName
{
    //Make sure your animation name follow this
    public const string IdleRight = "IdleRight";
    public const string IdleLeft = "IdleLeft";
}

public abstract class Controller : MonoBehaviour
{
    [SerializeField] private Collider2DComponent _shadow;

    private void OnEnable()
    {
        _shadow.triggerEnter += ShadowOntriggerEnter;
    }

    private void OnDisable()
    {
        _shadow.triggerEnter -= ShadowOntriggerEnter;
    }

    private void ShadowOntriggerEnter(Collider2D obj)
    {
    }
}
