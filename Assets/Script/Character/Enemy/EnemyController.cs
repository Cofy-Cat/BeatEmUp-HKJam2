using System;
using cfEngine.Logging;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController: Controller
{
    [SerializeField] private GameObject player;
    private Vector3 direction;
    private Vector2 input;

    protected override void Awake(){
        base.Awake();
        input = Vector2.zero;
        direction = Vector3.zero;
    }
    private void Start()
    {
        _command.ExecuteCommand(new IdleCommand());
    }

    private void FixedUpdate() {
        direction = player.transform.position - transform.position;
        direction.Normalize();
        Debug.Log($"Input: {input}");
        Debug.Log($"Direction xy: {direction.x}, {direction.y}");

        if (input != new Vector2(direction.x, direction.y)) {
            _command.ExecuteCommand(new IdleCommand());
            Debug.Log("Move");
            input = new Vector2(direction.x, direction.y);
            _command.ExecuteCommand(new MoveCommand(input));
        }
    }
}
