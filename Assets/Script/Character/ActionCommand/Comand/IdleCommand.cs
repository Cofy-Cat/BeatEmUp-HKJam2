public class IdleCommand: ActionCommand
{
    public override CommandType type => CommandType.Idle;

    protected override bool _Execute(in ExecutionContext context)
    {
        base.TryExecute(context);
        
        var sm = context.Controller.StateMachine;
        var command = context.Controller;
        if (!sm.CanGoToState(CharacterStateId.Idle))
        {
            return false;
        }

        context.Controller.StateMachine.GoToState(CharacterStateId.Idle);
        return true;
    }
}