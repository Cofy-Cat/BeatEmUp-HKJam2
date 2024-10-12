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

        MoveCommand newestMove = null;

        foreach (var command in commandQueue)
        {
            if (newestMove == null)
            {
                if (command is MoveCommand newestMoveCommand)
                {
                    newestMove = newestMoveCommand;
                }
                else
                {
                    return false;
                }
                
                continue;
            }

            if(command is not MoveCommand moveCommand) continue;

            if ((newestMove.Context.ExecutionTime - moveCommand.Context.ExecutionTime < _maxExecutionGap) 
                && newestMove.Direction == moveCommand.Direction)
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