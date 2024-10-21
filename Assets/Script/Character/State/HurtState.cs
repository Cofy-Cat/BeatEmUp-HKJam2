using System;
using System.Collections.Generic;
using cfEngine.Util;

public class HurtState : CharacterState
{
    public override HashSet<CharacterStateId> Whitelist { get; } = new() { };

    public override CharacterStateId Id => CharacterStateId.Hurt;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        string animationName = string.Empty;
        if (sm.Controller.LastFaceDirection >= 0)
        {
            animationName = AnimationName.HurtRight;
        }
        else if (sm.Controller.LastFaceDirection < 0)
        {
            animationName = AnimationName.HurtLeft;
        }

        if (sm.Controller is BossEnemyController)
        {
            sm.GoToState(CharacterStateId.Idle, checkWhitelist: false);
        }
        else
        {
            sm.Controller.Animation.Play(animationName, false, onAnimationEnd: () =>
            {
                sm.GoToState(CharacterStateId.Idle, checkWhitelist: false);
            });
        }
    }
}