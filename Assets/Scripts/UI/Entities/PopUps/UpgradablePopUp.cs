using Infrastructure.Data;
using Infrastructure.Services.Guid;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.StaticDataService;
using TMPro;
using UI.Entities.Windows;
using UnityEngine;
using Zenject;

namespace UI.Entities.PopUps
{
    public abstract class UpgradablePopUp : Window, ISavedProgressReader
    {
        [SerializeField] private TextMeshProUGUI _currentLevelText;
        
        private int _currentLevel = 1;
        private string _id;
        
        protected int _upgradeLevelsCount;

        private bool IsFinalLevel => _currentLevel == _upgradeLevelsCount - 1;
        
        protected abstract void OnFinalLevelReached();
        
        [Inject]
        public void Construct(IGuidService guidService, ISaveLoadService saveLoad)
        {
            saveLoad.Register(this);
            
            _id = guidService.GetGuidFor(gameObject);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _currentLevel = progress.GetBuildingSaveData(_id).BuildingLevel;
            
            SetLevel(_currentLevel);
        }

        protected virtual void SetLevel(int level)
        {
            _currentLevelText.text = $"Level {level.ToString()}";
        }

        public void OnProgressCouldNotBeLoaded()
        {
            SetLevel(_currentLevel);
        }

        protected void LevelUp()
        {
            _currentLevel++;
            
            if (IsFinalLevel)
            {
                OnFinalLevelReached();
            }

            SetLevel(_currentLevel);
        }
    }
}