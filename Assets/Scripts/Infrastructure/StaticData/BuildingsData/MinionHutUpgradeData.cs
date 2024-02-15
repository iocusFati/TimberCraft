using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.StaticData.BuildingsData
{
    [CreateAssetMenu(fileName = "MinionHutUpgradeData", menuName = "StaticData/Game/MinionHutUpgradeData")]
    public class MinionHutUpgradeData : ScriptableObject
    {
        [SerializeField] private List<LevelUpgrade> _levelUpgrades;

        public List<LevelUpgrade> LevelUpgrades => _levelUpgrades;

        public bool LevelIsTheLastOne(int level) => 
            level == _levelUpgrades.Count;
    }

    [Serializable]
    public class LevelUpgrade
    {
        public int MinionsQuantity;
        public int LootQuantity;
        public int Cost;
    }
}