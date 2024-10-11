public class IdleCommand: ActionCommand
{
    public override void Execute(ActionCommandController controller)
    {
        controller.StateMachine.GoToState(CharacterStateId.Idle);
    }
}