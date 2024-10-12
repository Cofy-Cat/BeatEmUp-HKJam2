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
        if (commandQueue.Count <= 1) return false;

        var newestMove = commandQueue[0];

        foreach (var command in commandQueue.Skip(1))
        {
            if(command.type != CommandType.Move) continue;

            if (newestMove.Context.ExecutionTime - command.Context.ExecutionTime < _maxExecutionGap)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}