using System.Collections.Generic;
using System.Linq;

public class DashPattern : CommandPattern
{
    private readonly float _maxExecutionGap;

    public DashPattern(float maxExecutionGap)
    {
        _maxExecutionGap = maxExecutionGap;
    }
    
    public override CommandType commandType => CommandType.Move;

    public override bool IsMatch(IReadOnlyList<ActionCommand> commandQueue)
    {
        if (commandQueue.Count < 3 || commandQueue[0] is not MoveCommand newestMove)
            return false;

        if (commandQueue[1] is not IdleCommand idleCommand || commandQueue[2] is not MoveCommand moveCommand)
            return false;

        return newestMove.Direction == moveCommand.Direction &&
               newestMove.Context.ExecutionTime - moveCommand.Context.ExecutionTime < _maxExecutionGap;
    }
}