public class EnemyController: Controller
{
    private void Start()
    {
        _command.ExecuteCommand(new IdleCommand());
    }
}
