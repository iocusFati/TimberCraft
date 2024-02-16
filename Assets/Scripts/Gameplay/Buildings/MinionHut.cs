using System.Collections.Generic;
using Gameplay.Bots;
using Gameplay.Locations;
using Gameplay.Resource.ResourceStorage;
using Infrastructure.Factories;
using Infrastructure.Factories.BotFactoryFolder;
using Infrastructure.Services.Guid;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.BuildingsData;
using UI.Mediator;
using UnityEngine;
using Zenject;

namespace Gameplay.Buildings
{
    public class MinionHut : UpgradableBuilding
    {
        [Header("Minion hut")]
        public Transform TriggerZoneTransform;

        private IUIMediator _uiMediator;
        private BotFactory _botFactory;
        private MinionHutUpgradeData _upgradeData;
        private ResourceSourcesHolder _resourceSourcesHolder;

        private readonly List<LumberjackBot> _bots = new();

        [Inject]
        public void Construct(IFactoriesHolderService factoriesHolder,
            IStaticDataService staticData,
            IUIMediator uiMediator,
            IGameResourceStorage gameResourceStorage, 
            IGuidService guidService)
        {
            base.Construct(gameResourceStorage, guidService);
            
            _botFactory = factoriesHolder.BotFactory;
            _uiMediator = uiMediator;
            _upgradeData = staticData.MinionHutUpgradeData;
        }

        public void Construct(ResourceSourcesHolder resourceSourcesHolder)
        {
            _resourceSourcesHolder = resourceSourcesHolder;
        }

        public override void InteractWithPlayer()
        {
            _uiMediator.SwitchMinionHutPopUp(this, show: true); 
        }

        public override void StopInteractingWithPlayer()
        {
            _uiMediator.SwitchMinionHutPopUp(this, show: false); 
        }

        protected override void Upgrade()
        {
            base.Upgrade();

            LevelUpgrade previousUpgradeData = GetLevelUpgradeData(_currentLevel);
            
            PayForUpgrade(previousUpgradeData.Cost);

            _currentLevel++;

            SpawnBotsUpgrade(_currentLevel);
        }

        protected override void SetLevel(int level)
        {
            base.SetLevel(level);
            
            SpawnBotsForLevel(level);
        }

        protected override void OnBuilt()
        {
            base.OnBuilt();
            
            SetLevel(1);
        }

        private void SpawnBotsUpgrade(int level)
        {
            LevelUpgrade previousUpgradeData = GetLevelUpgradeData(level - 1);
            LevelUpgrade currentUpgradeData = GetLevelUpgradeData(level);
            
            int spawnBotsQuantity = currentUpgradeData.MinionsQuantity - previousUpgradeData.MinionsQuantity;
            
            SpawnBots(spawnBotsQuantity);
            UpdateBotStorageCapacity(currentUpgradeData.LootQuantity);
        }
        
        private void SpawnBotsForLevel(int level)
        {
            LevelUpgrade currentUpgradeData = GetLevelUpgradeData(level);
            
            SpawnBots(currentUpgradeData.MinionsQuantity);
            UpdateBotStorageCapacity(currentUpgradeData.LootQuantity);
        }

        private LevelUpgrade GetLevelUpgradeData(int level) => 
            _upgradeData.LevelUpgrades[level - 1];

        private void UpdateBotStorageCapacity(int capacity)
        {
            foreach (var bot in _bots) 
                bot.BotStorage.StorageCapacity = capacity;
        }

        private void SpawnBots(int number)
        {
            for (int i = 0; i < number; i++) 
                _bots.Add(_botFactory.CreateLumberjackBotFrom(this, _resourceSourcesHolder));
        }
    }
}