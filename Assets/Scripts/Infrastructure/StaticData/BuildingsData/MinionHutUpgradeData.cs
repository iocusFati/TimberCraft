using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.StaticData.BuildingsData
{
    [CreateAssetMenu(fileName = "MinionHutUpgradeData", menuName = "StaticData/Game/MinionHutUpgradeData")]
    public class MinionHutUpgradeData : ScriptableObject
    {
        [SerializeField] private List<MinionHutLevelUpgrade> _levelUpgrades;

        public List<MinionHutLevelUpgrade> LevelUpgrades => _levelUpgrades;

        public bool LevelIsTheLastOne(int level) => 
            level == _levelUpgrades.Count - 1;
    }
}