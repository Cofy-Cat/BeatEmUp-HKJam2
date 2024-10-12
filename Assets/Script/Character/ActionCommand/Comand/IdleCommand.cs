public class IdleCommand: ActionCommand
{
    public override CommandType type => CommandType.Idle;

    public override void Execute(in ExecutionContext context)
    {
        base.Execute(context);
        context.Controller.StateMachine.GoToState(CharacterStateId.Idle);
    }
}