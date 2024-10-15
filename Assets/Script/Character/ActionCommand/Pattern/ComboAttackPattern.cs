using System.Collections.Generic;
using System.Linq;

public class ComboAttackPattern: CommandPattern
{
    public readonly string[] Combos;
    public readonly int Priority;
    public readonly float RepeatedMaxInterval;
    
    public override CommandType commandType => CommandType.Attack;
    
    public ComboAttackPattern(string[] combos, int priority, float repeatedMaxInterval)
    {
        Combos = combos;
        Priority = priority;
        RepeatedMaxInterval = repeatedMaxInterval;
    }
    
    public override bool IsMatch(ActionCommand newCommand, IReadOnlyList<ActionCommand> commandQueue)
    {
        if (newCommand is not AttackCommand newAttack)
        {
            return false;
        }

        if (Combos.Length > 1 && !Combos[^1].Equals(newAttack.AttackId))
        {
            return false;
        }

        if (Combos.Length == 1 && Combos[0].Equals(newAttack.AttackId))
        {
            return true;
        }

        var comboPointerIndex = Combos.Length - 2;
        var attackTime = newAttack.Context.ExecutionTime;
        for (var i = 0; i < commandQueue.Count; i++)
        {
            if(comboPointerIndex == -1) 
                break;
            
            if (commandQueue[i] is ComboAttackCommand comboAttackCommand)
            {
                if(comboAttackCommand.Combo.SequenceEqual(Combos))
                    return false;
            }
            
            if(commandQueue[i] is not AttackCommand attackCommand)
                continue;


            if (attackTime - attackCommand.Context.ExecutionTime > RepeatedMaxInterval)
            {
                return false;
            }
            attackTime = attackCommand.Context.ExecutionTime;

            if (!attackCommand.AttackId.Equals(Combos[comboPointerIndex]))
            {
                return false;
            }

            comboPointerIndex--;
        }

        return comboPointerIndex == -1;
    }
}