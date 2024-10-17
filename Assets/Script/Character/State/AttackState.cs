using UnityEngine;

public class AttackState: CharacterState
{
    public class Param : StateParam
    {
        public string[] Combo;
    }
    
    public override CharacterStateId[] stateBlacklist => new[] { CharacterStateId.Move, CharacterStateId.Idle, CharacterStateId.Attack };
    public override CharacterStateId Id => CharacterStateId.Attack;
    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        var p = (Param)param;

        var controller = sm.Controller;
        controller.SetVelocity(Vector2.zero);
        string animationName = AnimationName.GetComboDirectional(AnimationName.Attack, p.Combo, controller.LastFaceDirection);
        
        if (controller.AttackConfig == null || !controller.AttackConfig.tryGetConfig(animationName, out var config))
        {
            controller.Animation.Play(animationName, onAnimationEnd: () =>
            {
                controller.Attack();
                sm.GoToState(CharacterStateId.AttackEnd, p);
            });
        }
        else
        {
            controller.Animation.Play(animationName,
                onPlayFrame: frame =>
                {
                    if (frame == config.hitFrame)
                    {
                        controller.Attack();
                    }
                },
                onAnimationEnd: () =>
                {
                    sm.GoToState(CharacterStateId.AttackEnd, p);
                }
            );
        }
    }
}