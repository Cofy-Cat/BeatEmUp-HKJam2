using cfEngine.Util;

public class BattleState: GameState
{
    public override GameStateId Id => GameStateId.Battle;
    protected internal override void StartContext(StateMachine<GameStateId> sm, cfEngine.Util.StateParam param)
    {
        
    }
}