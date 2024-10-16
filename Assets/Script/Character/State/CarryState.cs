using UnityEngine.Assertions;

public class CarryState: CharacterState
{
    public override CharacterStateId[] stateBlacklist => new[] { CharacterStateId.Attack, CharacterStateId.AttackEnd };

    public class Param : StateParam
    {
        public Throwable Throwable;
    }
    
    public override CharacterStateId Id => CharacterStateId.Carry;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        var p = param as Param;
        Assert.IsNull(p);

        var anim = AnimationName.GetDirectional(AnimationName.Carry, sm.Controller.LastFaceDirection);
        sm.Controller.Animation.Play(anim);
    }
}