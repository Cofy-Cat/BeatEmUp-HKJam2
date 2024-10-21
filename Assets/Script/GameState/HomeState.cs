using cfEngine.Util;
using UnityEngine.SceneManagement;

public class HomeState: GameState
{
    public override GameStateId Id => GameStateId.Home;
    protected internal override void StartContext(GameStateMachine gsm, StateParam param)
    {
        if (SceneManager.GetActiveScene().name != "HomeScene")
        {
            SceneManager.LoadScene("HomeScene");
        }
    }
}