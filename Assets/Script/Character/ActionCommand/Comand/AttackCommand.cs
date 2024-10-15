using DocumentFormat.OpenXml.Office2010.Excel;

public class AttackCommand: ActionCommand
{
    public readonly string AttackId;
    public override CommandType type => CommandType.Attack;

    public AttackCommand(string id)
    {
        AttackId = id;
    }
    
    protected override bool _Execute(in ExecutionContext context)
    {
        var sm = context.Controller.StateMachine;

        if (!sm.CanGoToState(CharacterStateId.Attack))
        {
            return false;
        }
        
        var patterns = context.MatchedPatterns;
        ComboAttackPattern prioritizedCombo = null;
        foreach (var pattern in patterns)
        {
            if (pattern is not ComboAttackPattern comboAttack) continue;

            if (prioritizedCombo == null || comboAttack.Priority > prioritizedCombo.Priority)
            {
                prioritizedCombo = comboAttack;
            }
        }

        if (prioritizedCombo != null)
        {
            context.Controller.ExecuteCommand(new ComboAttackCommand(prioritizedCombo.Combos));
        }
        else
        {
            sm.GoToState(CharacterStateId.Attack, new AttackState.Param()
            {
                Combo = null
            });
        }

        return true;
    }

    public override string ToString()
    {
        return $"{nameof(AttackCommand)}-{AttackId}";
    }
}