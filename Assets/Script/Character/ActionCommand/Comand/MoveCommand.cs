using System.Linq;
using cfEngine.Logging;
using UnityEngine;

public class MoveCommand: ActionCommand
{
    public override CommandType type => CommandType.Move;
    
    private readonly Vector2 _direction;
    
    public MoveCommand(Vector2 direction)
    {
        _direction = direction;
    }

    public override void Execute(in ExecutionContext context)
    {
        base.Execute(context);
        
        context.Controller.StateMachine.GoToState(CharacterStateId.Move, new MoveState.Param()
        {
            direction = _direction
        });
    }
}