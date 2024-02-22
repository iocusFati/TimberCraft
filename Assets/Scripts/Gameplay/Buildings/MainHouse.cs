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
        
        [OdinSerialize, ListDrawerSettings(ShowIndexLabels = true), 
         ValidateInput("DataCountShouldBeEqualToCostCount", 
             "Data count should be equal to cost count in StaticData/GameData/MainHouseUpgradeData")] 
        private List<ІslandHouseFacadePair> _upgradeData;
        
        [ListDrawerSettings(ShowIndexLabels = true, DraggableItems = true),
            ValidateInput("DataCountShouldBeEqualToCostCount", 
                "Data count should be equal to cost count in StaticData/GameData/MainHouseUpgradeData")]
        [SerializeField] private List<List<GameObject>> _walls;

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
            
            UnlockIsland(_currentLevel);
            UpdateHouseLook(_currentLevel);
        }

        public override int GetCurrentUpgradeCost() => 
            _upgradeCosts[_currentLevel];

        protected override void SetLevel(int level)
        {
            base.SetLevel(level);

            UnlockAllIslandsUntil(level);
            UpdateHouseLook(level);
        }

        private void UnlockAllIslandsUntil(int islandIndex)
        {
            for (int i = 1; i <= islandIndex; i++) 
                UnlockIsland(i);
        }

        private void UnlockIsland(int islandIndex)
        {
            _upgradeData[islandIndex].UnlockIsland.gameObject.SetActive(true);

            DeactivateWalls(islandIndex);
        }

        private void DeactivateWalls(int islandIndex)
        {
            if (_walls.Count == 0)
                return;
            
            foreach (var wall in _walls[islandIndex]) 
                wall.SetActive(false);
        }

        private void UpdateHouseLook(int facadeIndex)
        {
            _upgradeData[facadeIndex].UpgradeMainHouseFacade.SetActive(true);
            
            if (facadeIndex > 0) 
                _upgradeData[facadeIndex - 1].UpgradeMainHouseFacade.SetActive(false);
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