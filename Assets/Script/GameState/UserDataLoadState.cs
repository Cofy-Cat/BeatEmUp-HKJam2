using cfEngine.Logging;
using cfEngine.Util;

public class UserDataLoadState: GameState
{
    public override GameStateId Id => GameStateId.UserDataLoad;
    protected internal override void StartContext(StateMachine<GameStateId> sm, cfEngine.Util.StateParam param)
    {
        Game.UserData.Register(Game.Meta.Statistic);

        Game.UserData.LoadInitializeAsync(Game.TaskToken).ContinueWith(t =>
        {
            if (t.IsCompletedSuccessfully)
            {
                sm.GoToState(GameStateId.Initialization);
            }
        });
    }
}