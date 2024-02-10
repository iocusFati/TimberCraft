using UnityEngine;

namespace Infrastructure.StaticData.LumberjackData
{
    [CreateAssetMenu(fileName = "LumberjackBotConfig", menuName = "StaticData/Configs/LumberjackBotConfig")]
    public class LumberjackBotConfig : ScriptableObject
    {
        [Header("Loot bag")]
        [SerializeField] private int _initialStorageCapacity;
        [SerializeField] private float _lootPositionOffset;
        [SerializeField] private float _storageFloors;

        public int InitialStorageCapacity => _initialStorageCapacity;

        public float LootPositionOffset => _lootPositionOffset;

        public float StorageFloors => _storageFloors;
    }
}