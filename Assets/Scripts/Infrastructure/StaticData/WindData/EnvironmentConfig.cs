using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Infrastructure.StaticData.WindData
{
    [CreateAssetMenu(fileName = "EnvironmentConfig", menuName = "StaticData/Configs/EnvironmentConfig")]
    public class EnvironmentConfig : SerializedScriptableObject
    {
        [Header("General")]
        [SerializeField, FoldoutGroup("Wind")] private LayerMask _effectLayer;
        [SerializeField, FoldoutGroup("Wind")] private float _duration;
        
        [Header("Force")]
        [SerializeField, FoldoutGroup("Wind")] private float _minForceMagnitude;
        [SerializeField, FoldoutGroup("Wind")] private float _maxForceMagnitude;

        [Header("Cooldown")]
        [SerializeField, FoldoutGroup("Wind")] private float _minCooldown;
        [SerializeField, FoldoutGroup("Wind")] private float _maxCooldown;

        [Header("Particle")]
        [SerializeField, FoldoutGroup("Wind")] private short _burstCount;
        [SerializeField, FoldoutGroup("Wind")] private float _repeatInterval;
        [SerializeField, FoldoutGroup("Wind")] private short _cycleCount;
        
        
        [SerializeField, FoldoutGroup("Bird")] private Tuple<float, float> _birdAppearCooldownMinMax;

        public float MinForceMagnitude => _minForceMagnitude;

        public float MaxForceMagnitude => _maxForceMagnitude;

        public LayerMask EffectLayer => _effectLayer;

        public float Duration => _duration;

        public short BurstCount => _burstCount;

        public float RepeatInterval => _repeatInterval;

        public short CycleCount => _cycleCount;

        public float MinCooldown => _minCooldown;

        public float MaxCooldown => _maxCooldown;

        public Tuple<float, float> BirdAppearCooldown => _birdAppearCooldownMinMax;
    }
}