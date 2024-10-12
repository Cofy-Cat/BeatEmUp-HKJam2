using cfEngine.Util;

public enum GameStateId
{
    InfoLoad,
    Login
}

public abstract class GameState : State<GameStateId>
{
}

public class GameStateMachine: StateMachine<GameStateId>
{
    public GameStateMachine(): base()
    {
        RegisterState(new InfoLoadState());
    }
}