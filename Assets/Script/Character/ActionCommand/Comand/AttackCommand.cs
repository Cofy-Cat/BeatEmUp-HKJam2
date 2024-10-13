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
        
        var sm = context.Controller.StateMachine;

        if (!sm.CanGoToState(CharacterStateId.Attack))
        {
            if (sm.CurrentState.Id == CharacterStateId.AttackEnd)
            {
                context.Controller.QueuePending(new AttackCommand());
            }

            return false;
        }
        else if(!context.Controller.TryDispatchPending())
        {
            context.Controller.StateMachine.GoToState(CharacterStateId.Attack, new AttackState.Param()
            {
                Combo = highestCombo?.RepeatedCount ?? 1
            });
            return true;
        }

        return true;
    }
}