using System.Collections.Generic;
using System.Linq;
using cfEngine.Logging;

public class DashPattern : CommandPattern
{
    private readonly float _maxExecutionGap;

    public DashPattern(float maxExecutionGap)
    {
        _maxExecutionGap = maxExecutionGap;
    }
    
    public override CommandType commandType => CommandType.Move;

    public override bool IsMatch(ActionCommand newCommand, IReadOnlyList<ActionCommand> commandQueue)
    {
        if (commandQueue.Count < 2 || newCommand is not MoveCommand newMove)
            return false;

        if (commandQueue[0] is not IdleCommand idleCommand || commandQueue[1] is not MoveCommand moveCommand)
            return false;

        return newMove.Direction == moveCommand.Direction &&
               newMove.Context.ExecutionTime - moveCommand.Context.ExecutionTime < _maxExecutionGap;
    }
}