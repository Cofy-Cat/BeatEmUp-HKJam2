public class IdleCommand: ActionCommand
{
    public override void Execute(in ExecuteParam param)
    {
        param.Controller.StateMachine.GoToState(CharacterStateId.Idle);
    }
}