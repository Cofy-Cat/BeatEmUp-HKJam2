using System;

public class KnockBackCommand: ActionCommand
{
    public readonly int Direction;
    public readonly float Force;
    
    public KnockBackCommand(int direction, float force)
    {
        Direction = direction;
        Force = force;
    }
    
    public override CommandType type => CommandType.CrowdControl;
    protected override bool _Execute(in ExecutionContext context)
    {
        if (context.Controller.StateMachine.CanGoToState(CharacterStateId.KnockBack))
        {
            context.Controller.StateMachine.GoToState(CharacterStateId.KnockBack, new KnockBackState.Param()
            {
                Direction = Direction,
                Force = Force
            });
            return true;
        }

        return false;
    }
}