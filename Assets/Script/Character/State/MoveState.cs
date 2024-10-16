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
        float faceDirection = 0f;
        Vector2 direction = Vector2.zero;

        if (param is Param p)
        {
            faceDirection = p.direction.x;
            direction = p.direction;
        }
        else
        {
            faceDirection = sm.Controller.LastFaceDirection;
            direction = sm.Controller.LastMoveDirection;
        }

        string animationName = AnimationName.GetDirectional(AnimationName.Walk, faceDirection);

        sm.Controller.SetVelocity(direction * sm.Controller.moveSpeed);
        
        sm.Controller.Animation.Play(animationName, true);
    }
}