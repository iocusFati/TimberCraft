using System;
using System.Collections.Generic;
using Gameplay.Buildings;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Infrastructure.StaticData.BuildingsData
{
    [CreateAssetMenu(fileName = "MainHouseUpgradeData", menuName = "StaticData/Game/MainHouseUpgradeData")]
    public class MainHouseUpgradeData : ScriptableObject
    {
        [SerializeField] private List<int> _levelUpgradeCosts;

        public List<int> LevelUpgradeCosts => _levelUpgradeCosts;
    }

}