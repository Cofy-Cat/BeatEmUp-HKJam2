public class AttackCommand: ActionCommand
{
    public override CommandType type => CommandType.Attack;

    public override void Execute(in ExecutionContext context)
    {
        base.Execute(in context);
        
        context.Controller.StateMachine.GoToState(CharacterStateId.Attack);
    }
}