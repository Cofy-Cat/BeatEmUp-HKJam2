using UnityEngine;

public class CharacterController : MonoBehaviour
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
