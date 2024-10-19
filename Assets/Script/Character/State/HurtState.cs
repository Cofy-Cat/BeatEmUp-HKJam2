using System;
using System.Collections.Generic;

public class HurtState : CharacterState
{
    public List<CharacterStateId> _blackList = new List<CharacterStateId>()
    {
        CharacterStateId.Idle, CharacterStateId.Attack, CharacterStateId.Carry, CharacterStateId.Move,
        CharacterStateId.Throw
    };
    public override CharacterStateId[] stateBlacklist => _blackList.ToArray();

    public override CharacterStateId Id => CharacterStateId.Hurt;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        string animationName = String.Empty;
        if (sm.Controller.LastFaceDirection >= 0)
        {
            animationName = AnimationName.HurtRight;
        }
        else if (sm.Controller.LastFaceDirection < 0)
        {
            animationName = AnimationName.HurtLeft;
        }

        sm.Controller.Animation.Play(animationName, false, onAnimationEnd: () =>
        {
            _blackList.Remove(CharacterStateId.Idle);
            sm.GoToState(CharacterStateId.Idle);
            _blackList.Add(CharacterStateId.Idle);
        });
    }
}