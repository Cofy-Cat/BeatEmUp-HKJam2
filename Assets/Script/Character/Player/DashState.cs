
using UnityEngine;

public class DashState: CharacterState
{
    public class Param : StateParam
    {
        public Vector2 direction;
    }
    
    public override CharacterStateId Id => CharacterStateId.Dash;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        var p = (Param)param;

        string animationName = p.direction.x > 0 ? AnimationName.DashRight : AnimationName.DashLeft;

        sm.Controller.SetVelocity(p.direction * sm.Controller.dashSpeed);
        
        sm.Controller.Animation.playSpriteSwapAnimation(animationName, true);
    }
}