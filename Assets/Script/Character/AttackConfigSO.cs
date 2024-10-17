using System;
using System.Collections.Generic;
using System.Linq;
using cfEngine.Logging;
using UnityEngine;

namespace Script.Character
{
    [CreateAssetMenu(fileName = "AttackConfigSO", menuName = "BeatEmUp/AttackConfig", order = 0)]
    public class AttackConfigSO : ScriptableObject
    {
        [SerializeField] private List<AttackConfig> configs;

        private Dictionary<string, AttackConfig> configMap;

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
        public float stepOffset;

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
}