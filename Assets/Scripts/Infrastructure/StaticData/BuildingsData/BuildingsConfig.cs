using UnityEngine;

namespace Infrastructure.StaticData.BuildingsData
{
    [CreateAssetMenu(fileName = "BuildingsConfig", menuName = "StaticData/Configs/BuildingsConfig")]
    public class BuildingsConfig : ScriptableObject
    {
        [SerializeField] private int _minionHutInitialBotSpawnNumber;

        public int MinionHutInitialBotSpawnNumber => _minionHutInitialBotSpawnNumber;
    }
}