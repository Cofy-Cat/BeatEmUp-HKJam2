using cfEngine.Util;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class BattleState: GameState
{
    private AsyncOperation sceneLoadOperation;
    
    public class Param : cfEngine.Util.StateParam
    {
        public string sceneName;
        public HealthRecord playerHealth;
    }
    
    public override GameStateId Id => GameStateId.Battle;
    protected internal override void StartContext(GameStateMachine gsm, StateParam param)
    {
        var p = param as Param;
        Assert.IsNotNull(p);

        if (sceneLoadOperation != null)
        {
            return;
        }
        
        sceneLoadOperation = SceneManager.LoadSceneAsync(p.sceneName, LoadSceneMode.Single);

        void initPlayer(AsyncOperation op)
        {
            var player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            player.SetHealthValue(p.playerHealth);
            sceneLoadOperation.completed -= initPlayer;
            sceneLoadOperation = null;
        }

        sceneLoadOperation.completed += initPlayer;
    }
}