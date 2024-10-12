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

        var sm = context.Controller.StateMachine;

        if (!sm.CanGoToState(CharacterStateId.Move))
        {
            context.Controller.QueuePending(this);
            return;
        }

        var patterns = context.Patterns;
        foreach (var pattern in patterns)
        {
            if (pattern is DashPattern)
            {
                context.Controller.ExecuteCommand(new DashCommand(Direction));
                return;
            }
        }
        
        sm.GoToState(CharacterStateId.Move, new MoveState.Param()
        {
            direction = Direction
        });
    }
}