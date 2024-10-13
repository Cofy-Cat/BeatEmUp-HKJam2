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
        if (commandQueue.Count <= 0 && commandQueue[0] is not AttackCommand attackCommand)
        {
            return false;
        }

        int repeatedCount = 1;
        
        for (var i = 1; i < commandQueue.Count; i++)
        {
            if (commandQueue[i - 1] is not AttackCommand prev || commandQueue[i] is not AttackCommand next)
                break;
            
            if(next.Context.ExecutionTime - prev.Context.ExecutionTime < RepeatedMaxInterval) 
                break;

            repeatedCount += 1;
        }

        return repeatedCount >= RepeatedCount;
    }
}