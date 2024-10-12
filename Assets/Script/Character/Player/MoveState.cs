using UnityEngine;

public class MoveState: CharacterState
{
    public class Param : StateParam
    {
        public Vector2 direction;
    }
    
    public override CharacterStateId Id => CharacterStateId.Move;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        var p = (Param)param;

        string animationName = AnimationName.GetDirectional(AnimationName.Walk, p.direction.x);

        sm.Controller.SetVelocity(p.direction * sm.Controller.moveSpeed);
        
        sm.Controller.Animation.playSpriteSwapAnimation(animationName, true);
    }
}