using UnityEngine;

namespace Infrastructure.StaticData.PoolData
{
    [CreateAssetMenu(fileName = "PoolConfig", menuName = "StaticData/Configs/PoolConfig")]
    public class PoolConfig : ScriptableObject
    {
        [SerializeField] private int _particleInitialSpawnNumber;
        [SerializeField] private int _dropoutInitialSpawnNumber;

        public int ParticleInitialSpawnNumber => _particleInitialSpawnNumber;

        public int DropoutInitialSpawnNumber => _dropoutInitialSpawnNumber;
    }
}