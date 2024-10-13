public class AttackCommand: ActionCommand
{
    public override CommandType type => CommandType.Attack;

    public override void Execute(in ExecutionContext context)
    {
        base.Execute(in context);

        var sm = context.Controller.StateMachine;

        if (!sm.CanGoToState(CharacterStateId.Attack))
        {
            if (sm.CurrentState.Id == CharacterStateId.AttackEnd)
            {
                context.Controller.QueuePending(new AttackCommand());
            }
        }
        else if(!context.Controller.TryDispatchPending())
        {
            context.Controller.StateMachine.GoToState(CharacterStateId.Attack);
        }
    }
}