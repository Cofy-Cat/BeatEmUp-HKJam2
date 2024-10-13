using UnityEngine;

public class MoveCommand: ActionCommand
{
    public override CommandType type => CommandType.Move;
    
    public readonly Vector2 Direction;
    
    public MoveCommand(Vector2 direction)
    {
        Direction = direction;
    }

    protected override bool _Execute(in ExecutionContext context)
    {
        base.TryExecute(context);

        var sm = context.Controller.StateMachine;

        if (!sm.CanGoToState(CharacterStateId.Move))
        {
            return false;
        }

        var patterns = context.MatchedPatterns;
        foreach (var pattern in patterns)
        {
            if (pattern is DashPattern)
            {
                context.Controller.ExecuteCommand(new DashCommand(Direction));
                return true;
            }
        }
        
        sm.GoToState(CharacterStateId.Move, new MoveState.Param()
        {
            direction = Direction
        });

        return true;
    }
}