public class IdleCommand: ActionCommand
{
    public override void Perform(ActionCommandController controller)
    {
        controller.StateMachine.GoToState(CharacterStateId.Idle);
    }
}