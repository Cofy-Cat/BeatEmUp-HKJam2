using System.Collections.Generic;
using UnityEngine.Assertions;

public class CarryState: CharacterState
{
    public override HashSet<CharacterStateId> Whitelist { get; } = new() { CharacterStateId.Idle, CharacterStateId.Move };

    public class Param : StateParam
    {
        public Throwable Throwable;
    }
    
    public override CharacterStateId Id => CharacterStateId.Carry;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        var p = param as Param;
        Assert.IsNotNull(p);

        var anim = AnimationName.GetDirectional(AnimationName.Carry, sm.Controller.LastFaceDirection);
        sm.Controller.Animation.Play(anim);
        sm.Controller.AttachThrowable(p.Throwable);
    }
}