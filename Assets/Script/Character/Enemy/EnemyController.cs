using System;
using cfEngine.Logging;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController : Controller
{
    [SerializeField] private GameObject player;
    private Vector3 direction;
    private Vector2 input;
    private bool isTriggered = false;
    protected override void Awake()
    {
        base.Awake();
        input = Vector2.zero;
        direction = Vector3.zero;
    }
    private void OnEnable()
    {
        _shadow.triggerEnter += OnShadowTriggerEnter;
        _shadow.triggerExit += OnShadowTriggerExit;
    }
    private void OnDisable()
    {
        _shadow.triggerEnter -= OnShadowTriggerEnter;
        _shadow.triggerExit -= OnShadowTriggerExit;
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
        if (ifPlayerIsNear()) ChasePlayer();
        else _command.ExecuteCommand(new IdleCommand());
    }

    public void Hurt()
    {
        _command.ExecuteCommand(new HurtCommand());
    }

    private bool ifPlayerIsNear()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ChasePlayer()
    {
        direction = player.transform.position - transform.position;
        direction.Normalize();
        if (input != new Vector2(direction.x, direction.y) && !isTriggered)
        {
            input = new Vector2(direction.x, direction.y);
            _command.ExecuteCommand(new MoveCommand(input));
        }
    }
}
