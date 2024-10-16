public class HurtCommand : ActionCommand
{
    public override CommandType type => CommandType.Hurt;

    protected override bool _Execute(in ExecutionContext context)
    {
        context.Controller.StateMachine.GoToState(CharacterStateId.Hurt);

        return true;
    }
}