public class IdleCommand: ActionCommand
{
    public readonly bool ForceGoIdle;
    public IdleCommand(bool forceGoIdle = false)
    {
        this.ForceGoIdle = forceGoIdle;
    }
    
    public override CommandType type => CommandType.Idle;

    protected override bool _Execute(in ExecutionContext context)
    {
        var sm = context.Controller.StateMachine;
        var command = context.Controller;
        if (!sm.CanGoToState(CharacterStateId.Idle))
        {
            return false;
        }

        context.Controller.StateMachine.GoToState(CharacterStateId.Idle, checkWhitelist: ForceGoIdle);
        return true;
    }
}