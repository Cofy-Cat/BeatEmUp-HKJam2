using System;
using System.Collections.Generic;
using System.Linq;
using cfEngine.Logging;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AttackConfigSO", menuName = "BeatEmUp/AttackConfig", order = 0)]
public class AttackConfigSO : ScriptableObject
{
    [SerializeField] private List<AttackConfig> configs;

    private Dictionary<string, AttackConfig> configMap;

    private void OnValidate()
    {
        foreach (var config in configs)
        {
            if (config.targetHitAction == null || config.targetHitActionType != config.targetHitAction.type)
            {
                config.targetHitAction = config.targetHitActionType switch
                {
                    TargetHitActionType.None => new TargetHitAction(),
                    TargetHitActionType.KnockBack => new KnockBackAction(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }

    public bool tryGetConfig(string animationName, out AttackConfig config)
    {
        if (configMap == null)
        {
            configMap = configs.ToDictionary(config => config.animationName);
        }

        if (!configMap.TryGetValue(animationName, out config))
        {
            Log.LogWarning($"config not found for animation name {animationName}");
            return false;
        }

        return true;
    }
}

[Serializable]
public class AttackConfig
{
    public string animationName;
    public int hitFrame;
    public int attackEffectFrame = 0;
    public Vector2 attackMove;
    [FormerlySerializedAs("postAttackActionType")] public TargetHitActionType targetHitActionType = TargetHitActionType.None;
    [SerializeReference] 
    public TargetHitAction targetHitAction;

    public SoundSetting attackSound;
    public EffectSetting attackEffect;
    public SoundSetting hitSound;
    public EffectSetting hitEffect;
}

[Serializable]
public class SoundSetting
{
    public AudioClip sound;
    public float volume = 1f;
}

[Serializable]
public class EffectSetting
{
    public string effectName;
    public float speed = 1;
    public Vector2 offset;
    public Vector2 scale = Vector2.one;
    public Quaternion rotation = Quaternion.identity;
}

public enum TargetHitActionType
{
    None,
    KnockBack
}

[Serializable]
public class TargetHitAction
{
    public virtual TargetHitActionType type => TargetHitActionType.None;
    public virtual ActionCommand GetCommand()
    {
        return null;
    }
}

[Serializable]
public class KnockBackAction: TargetHitAction
{
    public override TargetHitActionType type => TargetHitActionType.KnockBack;

    public Vector2 force;
    public float distance;
    public float gravity;

    public override ActionCommand GetCommand()
    {
        return new KnockBackCommand(force, distance, gravity);
    }
}