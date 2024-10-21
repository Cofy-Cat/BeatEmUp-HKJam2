using System;
using System.Collections.Generic;
using cfEngine.Util;
using UnityEngine;

public class AttackState: CharacterState
{
    public class Param : StateParam
    {
        public string[] Combo;
    }

    public override HashSet<CharacterStateId> Whitelist { get; } = new() { CharacterStateId.AttackEnd , CharacterStateId.Hurt};
    public override CharacterStateId Id => CharacterStateId.Attack;

    protected internal override void StartContext(CharacterStateMachine sm, StateParam param)
    {
        var p = (Param)param;

        var controller = sm.Controller;
        controller.SetVelocity(Vector2.zero);

        string animationName;
        if (sm.LastStateId == CharacterStateId.Dash)
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
            if (config.attackSound.sound != null)
            {
                AudioManager.Instance.PlaySoundFXClip(config.attackSound.sound, config.attackSound.volume);
            }
                    
            var attackMoveVelocity = config.attackMove / controller.Animation.GetDuration(animationName);
            controller.Rigidbody.linearVelocity = attackMoveVelocity;
            controller.Animation.Play(
                animationName,
                speedMultiplier: config.animationSpeed,
                onPlayFrame: frame =>
                {
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
                    controller.Rigidbody.linearVelocity = Vector2.zero;
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
                AudioManager.Instance.PlaySoundFXClip(config.hitSound.sound, config.hitSound.volume);
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