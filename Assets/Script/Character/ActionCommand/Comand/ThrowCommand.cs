public class ThrowCommand: ActionCommand
{
    public override CommandType type => CommandType.Attack;
    protected override bool _Execute(in ExecutionContext context)
    {
        if (!context.Controller.StateMachine.Controller.isCarrying)
            return false;

        if (!context.Controller.StateMachine.CanGoToState(CharacterStateId.Throw))
        {
            return false;
        }
        
        context.Controller.StateMachine.GoToState(CharacterStateId.Throw);
        return true;
    }
}