using System.Linq;
using System.Threading.Tasks;
using cfEngine.Util;

public class InfoLoadState: GameState
{
    public override GameStateId Id => GameStateId.InfoLoad;
    protected internal override void StartContext(StateMachine<GameStateId> sm, cfEngine.Util.StateParam param)
    {
        RegisterInfos();
        
        var infoLoadTasks = Game.Info.InfoMap.Values.Select(info => info.LoadSerializedAsync(Game.TaskToken));
        Task.WhenAll(infoLoadTasks).ContinueWith(t =>
        {
            sm.GoToState(GameStateId.Login, new LoginState.Param()
            {
                Platform = LoginPlatform.Local,
                Token = new LoginToken()
            });
        }, Game.TaskToken, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
    }
    
    private void RegisterInfos()
    {
        //Add infos need to be loaded on init here
    }
}