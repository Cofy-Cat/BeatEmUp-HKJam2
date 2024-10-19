using cfEngine.Util;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class BattleState: GameState
{
    public class Param : cfEngine.Util.StateParam
    {
        public string sceneName;
        public HealthRecord playerHealth;
    }
    
    public override GameStateId Id => GameStateId.Battle;
    protected internal override void StartContext(StateMachine<GameStateId> sm, cfEngine.Util.StateParam param)
    {
        var p = param as Param;
        Assert.IsNotNull(p);
        
        var loadOp = SceneManager.LoadSceneAsync(p.sceneName, LoadSceneMode.Single);

        void initPlayer(AsyncOperation op)
        {
            var player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            player.SetHealthValue(p.playerHealth);
            loadOp.completed -= initPlayer;
        }

        loadOp.completed += initPlayer;
    }
}