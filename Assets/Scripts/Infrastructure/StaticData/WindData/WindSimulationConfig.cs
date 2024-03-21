using UnityEngine;

namespace Infrastructure.StaticData.WindData
{
    [CreateAssetMenu(fileName = "WindSimulationConfig", menuName = "StaticData/Configs/WindSimulationConfig")]
    public class WindSimulationConfig : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private LayerMask _effectLayer;
        [SerializeField] private float _duration;
        
        [Header("Force")]
        [SerializeField] private float _minForceMagnitude;
        [SerializeField] private float _maxForceMagnitude;

        [Header("Cooldown")]
        [SerializeField] private float _minCooldown;
        [SerializeField] private float _maxCooldown;

        [Header("Particle")]
        [SerializeField] private short _burstCount;
        [SerializeField] private float _repeatInterval;
        [SerializeField] private short _cycleCount;

        public float MinForceMagnitude => _minForceMagnitude;

        public float MaxForceMagnitude => _maxForceMagnitude;

        public LayerMask EffectLayer => _effectLayer;

        public float Duration => _duration;

        public short BurstCount => _burstCount;

        public float RepeatInterval => _repeatInterval;

        public short CycleCount => _cycleCount;

        public float MinCooldown => _minCooldown;

        public float MaxCooldown => _maxCooldown;
    }
}