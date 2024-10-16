public class CarryCommand: ActionCommand
{
    public readonly Throwable Throwable;
    public override CommandType type => CommandType.Attack;

    public CarryCommand(Throwable throwable)
    {
        Throwable = throwable;
    }
    
    protected override bool _Execute(in ExecutionContext context)
    {
        var sm = context.Controller.StateMachine;
        if (!sm.CanGoToState(CharacterStateId.Carry)) return false;
        
        sm.GoToState(CharacterStateId.Carry, new CarryState.Param
        {
            Throwable = Throwable
        });
        return true;
    }
}