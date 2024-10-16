
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

        string animationName = AnimationName.GetDirectional(AnimationName.Dash, p.direction.x);

        sm.Controller.SetVelocity(p.direction * sm.Controller.dashSpeed);
        
        sm.Controller.Animation.playSpriteSwapAnimation(animationName, true);
    }
}