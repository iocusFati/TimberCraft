using System;
using System.Collections.Generic;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.BuildingsData;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UI.Mediator;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Buildings
{
    public class MainHouse : UpgradableBuilding
    {
        [Header("Main house")]
        [OdinSerialize, ValidateInput("DataCountShouldBeEqualToCostCount",
            "Data count should be equal to cost count in StaticData/GameData/MainHouseUpgradeData")] 
        private List<ІslandHouseFacadePair> _upgradeData;

        private IUIMediator _uiMediator;
        private IGameCameraController _gameCameraController;
        private List<int> _upgradeCosts;

        [Inject]
        public void Construct(IUIMediator uiMediator,
            IGameCameraController gameCameraController,
            IStaticDataService staticData)
        {
            _uiMediator = uiMediator;
            _gameCameraController = gameCameraController;

            _upgradeCosts = staticData.MainHouseUpgradeData.LevelUpgradeCosts;
        }

        public override void InteractWithPlayer()
        {
            _uiMediator.SwitchMainHousePopUp(true);
            _gameCameraController.SwitchToTopViewCamera();
        }

        public override void StopInteractingWithPlayer()
        {
            _uiMediator.SwitchMainHousePopUp(false);
            _gameCameraController.SwitchToPlayerCamera();
        }

        public override void Upgrade()
        {
            base.Upgrade();
            
            PayForUpgrade(_upgradeCosts[_currentLevel]);
            
            UnlockIsland();
            UpdateHouseLook();
        }

        public override int GetCurrentUpgradeCost() => 
            _upgradeCosts[_currentLevel];

        private void UnlockIsland()
        {
            _upgradeData[_currentLevel].UnlockIsland.gameObject.SetActive(true);
        }

        private void UpdateHouseLook()
        {
            _upgradeData[_currentLevel].UpgradeMainHouseFacade.SetActive(true);
            
            if (_currentLevel > 1) 
                _upgradeData[_currentLevel - 1].UpgradeMainHouseFacade.SetActive(true);
        }

        private bool DataCountShouldBeEqualToCostCount() => 
            Resources.Load<MainHouseUpgradeData>(AssetPaths.MainHouseUpgradeData).LevelUpgradeCosts.Count == _upgradeData.Count;
    }

    [Serializable]
    public class ІslandHouseFacadePair
    {
        public GameObject UnlockIsland;
        public GameObject UpgradeMainHouseFacade;
    }
}