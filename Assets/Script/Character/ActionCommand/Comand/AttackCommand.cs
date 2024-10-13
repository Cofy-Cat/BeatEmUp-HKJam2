using System.Linq;
using cfEngine.Logging;

public class AttackCommand: ActionCommand
{
    public override CommandType type => CommandType.Attack;

    protected override bool _Execute(in ExecutionContext context)
    {
        var patterns = context.MatchedPatterns;
        foreach (var pattern in patterns)
        {
            if (pattern is RepeatAttackPattern)
            {
                Log.LogInfo("Detect combo attack!");
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
            context.Controller.StateMachine.GoToState(CharacterStateId.Attack);
            return true;
        }

        return true;
    }
}