public class HurtCommand : ActionCommand
{
    public override CommandType type => CommandType.Hurt;

    public override void Execute(in ExecutionContext context)
    {
        base.Execute(context);
        context.Controller.StateMachine.GoToState(CharacterStateId.Hurt);
    }
}