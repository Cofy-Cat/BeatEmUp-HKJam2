using System;
using UnityEngine;
using UnityEngine.Assertions;

public class Collider2DComponent : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    
    public event Action<Collider2D> triggerEnter;
    public event Action<Collider2D> triggerStay;
    public event Action<Collider2D> triggerExit;

    private void Awake()
    {
        if (_collider == null)
        {
            _collider = GetComponentInChildren<Collider2D>();
        }
        
        Assert.IsNotNull(_collider, $"There is no collider under {gameObject.name}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        triggerEnter?.Invoke(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        triggerStay?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        triggerStay?.Invoke(other);
    }

    public void GetTriggeringColliders()
    {
    }

    //TODO: can also add oncollisionenter later if needed
}
