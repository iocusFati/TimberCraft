using System;
using Gameplay.Resource;
using Gameplay.Resource.ResourceStorage;
using Infrastructure.Data;
using Infrastructure.Services.Guid;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.Buildings
{
    public abstract class UpgradableBuilding : Building, ISavedProgress
    {
        [SerializeField] private TextMeshPro _levelText;
        
        private IGameResourceStorage _gameResourceStorage;
        private IGuidService _guidService;

        protected int _currentLevel = 1;
        private string _guid;

        public abstract int GetCurrentUpgradeCost();

        [Inject]
        public void Construct(IGameResourceStorage gameResourceStorage, IGuidService guidService)
        {
            _gameResourceStorage = gameResourceStorage;
            
            _guid = guidService.GetGuidFor(gameObject);
        }

        private void Awake()
        {
            _saveLoadService.Register(this);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.TryAddBuildingSaveDataPair(_guid, new BuildingSaveData());
            
            BuildingSaveData saveData = progress.GetBuildingSaveData(_guid);
            
            saveData.BuildingLevel = _currentLevel;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            BuildingSaveData saveData = progress.GetBuildingSaveData(_guid);

            if (saveData is null)
                return;
            if (!saveData.IsBuilt)
                return;

            _currentLevel = saveData.BuildingLevel;
            SetLevel(_currentLevel);
        }

        public void OnProgressCouldNotBeLoaded()
        {
            // SetLevel(_currentLevel);
        }

        protected virtual void SetLevel(int level)
        {
            _currentLevel = level;
            
            SetLevelText(level);
        }

        public virtual void Upgrade()
        {
            _currentLevel++;

            SetLevelText(_currentLevel);
        }

        protected void PayForUpgrade(int cost) => 
            _gameResourceStorage.TakeResource(ResourceType.Coin, cost);

        protected override void OnBuilt()
        {
            base.OnBuilt();
            
            SetLevel(1);
        }

        private void SetLevelText(int level)
        {
            _levelText.text = $"Level {(level).ToString()}";
        }
    }
}