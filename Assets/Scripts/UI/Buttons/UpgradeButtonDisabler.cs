using Gameplay.Buildings;
using Gameplay.Environment.Buildings;
using UnityEngine;

namespace UI.Buttons
{
    public class UpgradeButtonDisabler : ButtonDisabler
    {
        [SerializeField] private UpgradableBuilding _upgradableBuilding;

        protected override bool CanBuy(int newResourceCount) => 
            newResourceCount >= _upgradableBuilding.GetCurrentUpgradeCost();
    }
}