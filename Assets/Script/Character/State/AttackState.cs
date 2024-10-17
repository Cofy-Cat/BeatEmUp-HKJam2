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

        string animationName;
        if (sm.LastState.Id == CharacterStateId.Dash)
        {
            animationName = AnimationName.GetDirectional(AnimationName.DashAttack, controller.LastFaceDirection);
        }
        else
        {
            animationName = AnimationName.GetComboDirectional(AnimationName.Attack, p.Combo, controller.LastFaceDirection);
        }

        if (controller.AttackConfig == null || !controller.AttackConfig.tryGetConfig(animationName, out var config))
        {
            controller.Animation.Play(animationName, onAnimationEnd: () =>
            {
                controller.Attack(null);
                sm.GoToState(CharacterStateId.AttackEnd, p);
            });
        }
        else
        {
            controller.Animation.Play(animationName,
                onPlayFrame: frame =>
                {
                    var controllerTransform = controller.transform;
                    controllerTransform.position = new Vector2(
                        transform.position.x + config.attackMove.x,
                        controllerTransform.position.y + config.attackMove.y / (config.hitFrame + 1));
                    
                    if (config.attackSound.sound != null)
                    {
                        AudioManager.Instance.PLaySoundFXClip(config.attackSound.sound, config.attackSound.volume);
                    }
                    
                    if (frame == config.attackEffectFrame && !string.IsNullOrEmpty(config.attackEffect.effectName))
                    {
                        playVfx(config.attackEffect);
                    }

                    if (frame == config.hitFrame)
                    {
                        PerformAttack(controller, config);
                    }
                },
                onAnimationEnd: () =>
                {
                    sm.GoToState(CharacterStateId.AttackEnd, p);
                }
            );
        }
    }

    private void PerformAttack(Controller controller, AttackConfig config)
    {
        var successHit = controller.Attack(config);

        if (successHit && !string.IsNullOrEmpty(config.hitEffect.effectName))
        {
            if (config.hitSound.sound != null)
            {
                AudioManager.Instance.PLaySoundFXClip(config.hitSound.sound, config.hitSound.volume);
            }
            playVfx(config.hitEffect);
        }
    }

    void playVfx(EffectSetting setting)
    {
        if (Game.Pool.TryGetPool("Vfx", out var pool) && pool is PrefabPool<SpriteAnimation> vfxPool)
        {
            var vfx = vfxPool.Get();
            vfx.gameObject.SetActive(true);
            var vfxTransform = vfx.transform;
            vfxTransform.position = new Vector2(transform.position.x + setting.offset.x,
                transform.position.y + setting.offset.y);
            vfxTransform.rotation = setting.rotation;
            vfxTransform.localScale = setting.scale;
            vfx.Play(setting.effectName, speedMultiplier: setting.speed,
                onAnimationEnd: () => { vfxPool.Release(vfx); });
        }
    }
}