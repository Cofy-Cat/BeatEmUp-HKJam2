public class IdleCommand: ActionCommand
{
    public override CommandType type => CommandType.Idle;

    public override void Execute(in ExecutionContext context)
    {
        base.Execute(context);
        
        var sm = context.Controller.StateMachine;
        var command = context.Controller;
        if (!sm.CanGoToState(CharacterStateId.Idle))
        {
            return;
        }

        context.Controller.StateMachine.GoToState(CharacterStateId.Idle);
    }
}