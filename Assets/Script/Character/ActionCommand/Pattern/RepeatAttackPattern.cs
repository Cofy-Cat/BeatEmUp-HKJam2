using System.Collections.Generic;

public class RepeatAttackPattern: CommandPattern
{
    public readonly int RepeatedCount;
    public readonly float RepeatedMaxInterval;
    
    public RepeatAttackPattern(int repeatedCount, float repeatedMaxInterval)
    {
        RepeatedCount = repeatedCount;
        RepeatedMaxInterval = repeatedMaxInterval;
    }
    
    public override CommandType commandType => CommandType.Attack;
    public override bool IsMatch(IReadOnlyList<ActionCommand> commandQueue)
    {
        if (commandQueue.Count <= 0 || commandQueue[0] is not AttackCommand thisAttack)
        {
            return false;
        }

        int repeatedCount = 1;

        AttackCommand next = thisAttack;
        
        for (var i = 1; i < commandQueue.Count; i++)
        {
            if(commandQueue[i] is IdleCommand)
                continue;

            if (commandQueue[i] is AttackCommand attackCommand)
            {
                if (next.Context.ExecutionTime - attackCommand.Context.ExecutionTime < RepeatedMaxInterval)
                {
                    repeatedCount += 1;
                    next = attackCommand;
                    continue;
                }
            }

            break;
        }

        return repeatedCount >= RepeatedCount;
    }
}