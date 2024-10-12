using System;
using cfEngine.Logging;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController : Controller
{
    [SerializeField] private GameObject player;
    private Vector3 direction;
    private Vector2 input;
    private Boolean isTriggered = false;

    protected override void Awake()
    {
        base.Awake();
        input = Vector2.zero;
        direction = Vector3.zero;
        _shadow.triggerEnter += OnShadowTriggerEnter;
        _shadow.triggerExit += OnShadowTriggerExit;
    }
    private void OnShadowTriggerEnter(Collider2D collider)
    {
        Debug.Log($"OnShadowTriggerEnter: " + collider.name);
        isTriggered = true;
        _command.ExecuteCommand(new IdleCommand());
    }

    private void OnShadowTriggerExit(Collider2D collider)
    {
        Debug.Log($"OnShadowTriggerExit: " + collider.name);
        isTriggered = false;
    }

    private void Start()
    {
        _command.ExecuteCommand(new IdleCommand());
    }

    private void FixedUpdate()
    {
        direction = player.transform.position - transform.position;
        direction.Normalize();

        if (input != new Vector2(direction.x, direction.y) && !isTriggered)
        {
            Debug.Log("Move");
            Debug.Log($"Input: {input}");
            Debug.Log($"Direction xy: {direction.x}, {direction.y}");
            input = new Vector2(direction.x, direction.y);
            _command.ExecuteCommand(new MoveCommand(input));
        }
    }
}
