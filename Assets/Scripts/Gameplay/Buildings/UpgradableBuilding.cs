using Gameplay.Resource;
using Infrastructure.Data;
using Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.Bots.StateMachine.States
{
    public abstract class UpgradableBuilding : Building, ISavedProgress
    {
        [SerializeField] private TextMeshPro _levelText;
        
        private IGameResourceStorage _gameResourceStorage;
        private IGuidService _guidService;

        protected int _currentLevel = 1;
        private string _guid;

        [Inject]
        public void Construct(IGameResourceStorage gameResourceStorage, IGuidService guidService)
        {
            _gameResourceStorage = gameResourceStorage;
            
            _guid = guidService.GetGuidFor(gameObject);
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
            if (saveData.IsBuilt)
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
            SetLevelText(level);
        }

        protected virtual void Upgrade()
        {
            SetLevelText(_currentLevel + 1);
        }

        protected void PayForUpgrade(int cost) => 
            _gameResourceStorage.TakeResource(ResourceType.Coin, cost);

        private void SetLevelText(int level)
        {
            _levelText.text = level.ToString();
        }
    }
}