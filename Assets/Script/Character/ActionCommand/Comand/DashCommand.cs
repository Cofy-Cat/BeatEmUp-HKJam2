using UnityEngine;

public class DashCommand: ActionCommand
{
    public readonly Vector2 _direction;
    
    public override CommandType type => CommandType.Move;

    public DashCommand(Vector2 direction)
    {
        _direction = direction;
    }
    
    protected override bool _Execute(in ExecutionContext context)
    {
        context.Controller.StateMachine.GoToState(CharacterStateId.Dash, new DashState.Param()
        {
            direction = _direction
        });

        return true;
    }
}