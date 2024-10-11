using UnityEngine;

public class EnemyController: Controller
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private void Start()
    {
        _command.ExecuteCommand(new IdleCommand());
    }

    
}
