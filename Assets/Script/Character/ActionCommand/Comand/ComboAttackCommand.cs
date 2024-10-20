public class ComboAttackCommand: ActionCommand
{
    public readonly string[] Combo;
    public override CommandType type => CommandType.Attack;

    public ComboAttackCommand(string[] combo)
    {
        Combo = combo;
    }
    
    protected override bool _Execute(in ExecutionContext context)
    {
        var sm = context.Controller.StateMachine;

        if (!sm.CanGoToState(CharacterStateId.Attack))
        {
            if (sm.CurrentStateId == CharacterStateId.AttackEnd)
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

    public override string ToString()
    {
        return $"{nameof(ComboAttackCommand)}-{string.Join("", Combo)}";
    }
}
    
       
    