public class ComboAttackCommand: ActionCommand
{
    public readonly int Combo;
    public override CommandType type => CommandType.Attack;

    public ComboAttackCommand(int combo)
    {
        Combo = combo;
    }
    
    protected override bool _Execute(in ExecutionContext context)
    {
        var sm = context.Controller.StateMachine;

        if (!sm.CanGoToState(CharacterStateId.Attack))
        {
            if (sm.CurrentState.Id == CharacterStateId.AttackEnd)
            {
                context.Controller.QueuePending(new ComboAttackCommand(Combo));
            }

            return false;
        }
        else if(!context.Controller.TryDispatchPending())
        {
            context.Controller.StateMachine.GoToState(CharacterStateId.Attack, new AttackState.Param()
            {
                Combo = Combo
            });
            return true;
        }

        return true;
    }
}
    
       
    