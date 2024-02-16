using Infrastructure.Data;
using Infrastructure.Services.Guid;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.BuildingsData;
using Sirenix.OdinInspector;
using TMPro;
using UI.Entities.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Entities.PopUps
{
    public class MinionHutPopUp : Window, ISavedProgressReader
    {
        [SerializeField] private Button _upgradeButton;
        
        [Title("Texts", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private TextMeshProUGUI _minionsQuantityText;
        [SerializeField] private TextMeshProUGUI _lootQuantityText;

        [Header("Upgrade values")]
        [SerializeField] private TextMeshProUGUI _minionsPlusText;
        [SerializeField] private TextMeshProUGUI _lootPlusText;

        [Header("Other")]
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private TextMeshProUGUI _currentLevelText;
        
        private MinionHutUpgradeData _minionHutUpgradeData;
        
        private int _currentLevel = 1;
        private string _id;

        [Inject]
        public void Construct(IStaticDataService staticData, IGuidService guidService, ISaveLoadService saveLoad)
        {
            base.Construct(staticData);
            
            saveLoad.Register(this);

            _minionHutUpgradeData = staticData.MinionHutUpgradeData;

            _id = guidService.GetGuidFor(gameObject);
        }

        private void Awake()
        {
            _upgradeButton.onClick.AddListener(LevelUp);
        }
        
        public void LoadProgress(PlayerProgress progress)
        {
            _currentLevel = progress.GetBuildingSaveData(_id).BuildingLevel;
            
            SetLevel(_currentLevel);
        }

        public void OnProgressCouldNotBeLoaded()
        {
            SetLevel(_currentLevel);
        }

        private void LevelUp()
        {
            _currentLevel++;
            SetLevel(_currentLevel);
        }

        private void SetLevel(int level)
        {
            _currentLevelText.text = $"Level {level.ToString()}";
            
            LevelUpgrade currentLevelUpgrade = _minionHutUpgradeData.LevelUpgrades[level - 1];

            _minionsQuantityText.text = currentLevelUpgrade.MinionsQuantity.ToString();
            _lootQuantityText.text = currentLevelUpgrade.LootQuantity.ToString();

            if (_minionHutUpgradeData.LevelIsTheLastOne(level))
            {
                _upgradeButton.enabled = false;
                return;
            }
            
            LevelUpgrade nextLevelUpgrade = _minionHutUpgradeData.LevelUpgrades[level];
            
            int minionsQuantityDifference = 
                nextLevelUpgrade.MinionsQuantity - currentLevelUpgrade.MinionsQuantity;
            int lootQuantityDifference = 
                nextLevelUpgrade.LootQuantity - currentLevelUpgrade.LootQuantity;

            _minionsPlusText.text = minionsQuantityDifference != 0 
                ? PlusText(minionsQuantityDifference) 
                : string.Empty;
            
            _lootPlusText.text = lootQuantityDifference != 0 
                ? PlusText(lootQuantityDifference) 
                : string.Empty;

            _costText.text = nextLevelUpgrade.Cost.ToString();
        }

        private static string PlusText(int quantity) => 
            $"+{quantity.ToString()}";
    }
}