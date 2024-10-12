using UnityEngine;

public class MoveCommand: ActionCommand
{
    public override CommandType type => CommandType.Move;
    
    public readonly Vector2 Direction;
    
    public MoveCommand(Vector2 direction)
    {
        Direction = direction;
    }

    public override void Execute(in ExecutionContext context)
    {
        base.Execute(context);

        var patterns = context.Patterns;
        foreach (var pattern in patterns)
        {
            if (pattern is DashPattern)
            {
                context.Controller.ExecuteCommand(new DashCommand(Direction));
                return;
            }
        }
        
        context.Controller.StateMachine.GoToState(CharacterStateId.Move, new MoveState.Param()
        {
            direction = Direction
        });
    }
}