using UnityEngine;

public class KnockBackCommand: ActionCommand
{
    public readonly Vector2 Force;
    public readonly float Distance;
    public readonly float Gravity;
    public readonly float AirboneFallStunDuration;

    public KnockBackCommand(Vector2 force, float distance, float gravity, float airboneFallStunDuration = 1)
    {
        Force = force;
        Distance = distance;
        Gravity = gravity;
        AirboneFallStunDuration = airboneFallStunDuration;
    }
    
    public override CommandType type => CommandType.CrowdControl;
    protected override bool _Execute(in ExecutionContext context)
    {
        if (context.Controller.StateMachine.CanGoToState(CharacterStateId.KnockBack))
        {
            context.Controller.StateMachine.GoToState(CharacterStateId.KnockBack, new KnockBackState.Param()
            {
                KnockBackDistance = Distance,
                Force = Force,
                Gravity = Gravity,
                AirboneFallStunDuration = AirboneFallStunDuration
            });
            return true;
        }

        return false;
    }
}