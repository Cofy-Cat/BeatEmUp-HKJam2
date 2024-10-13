public class AttackCommand: ActionCommand
{
    public override CommandType type => CommandType.Attack;

    protected override bool _Execute(in ExecutionContext context)
    {
        var patterns = context.MatchedPatterns;
        RepeatAttackPattern highestCombo = null;
        foreach (var pattern in patterns)
        {
            if (pattern is not RepeatAttackPattern comboAttack) continue;
            
            if (highestCombo == null || comboAttack.RepeatedCount > highestCombo.RepeatedCount)
            {
                highestCombo = comboAttack;
            }
        }
        
        context.Controller.ExecuteCommand(new ComboAttackCommand(highestCombo?.RepeatedCount ?? 1));

        return true;
    }
}