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

        for (var i = 1; i < commandQueue.Count; i++)
        {
            if(commandQueue[i] is not ComboAttackCommand comboAttack)
                continue;

            if (thisAttack.Context.ExecutionTime - comboAttack.Context.ExecutionTime < RepeatedMaxInterval
                && RepeatedCount == comboAttack.Combo + 1)
            {
                return true;
            }
        }

        return false;
    }
}