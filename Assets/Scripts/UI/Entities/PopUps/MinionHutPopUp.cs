using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.BuildingsData;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Entities.PopUps
{
    public class MinionHutPopUp : UpgradablePopUp
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
        
        private MinionHutUpgradeData _minionHutUpgradeData;

        [Inject]
        public new void Construct(IStaticDataService staticData)
        {
            _minionHutUpgradeData = staticData.MinionHutUpgradeData;
            _upgradeLevelsCount = _minionHutUpgradeData.LevelUpgrades.Count;
        }

        private void Awake()
        {
            _upgradeButton.onClick.AddListener(LevelUp);
        }

        protected override void OnFinalLevelReached()
        {
            _upgradeButton.gameObject.SetActive(false);
        }

        protected override void SetLevel(int level)
        {
            base.SetLevel(level);
            
            MinionHutLevelUpgrade currentLevelUpgrade = _minionHutUpgradeData.LevelUpgrades[level - 1];

            _minionsQuantityText.text = currentLevelUpgrade.MinionsQuantity.ToString();
            _lootQuantityText.text = currentLevelUpgrade.LootQuantity.ToString();

            if (IsTheLastLevel(level)) 
                return;
            
            MinionHutLevelUpgrade nextLevelUpgrade = _minionHutUpgradeData.LevelUpgrades[level];
            
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

        private bool IsTheLastLevel(int level)
        {
            if (_minionHutUpgradeData.LevelIsTheLastOne(level))
            {
                _upgradeButton.gameObject.SetActive(false);
                return true;
            }

            return false;
        }

        private static string PlusText(int quantity) => 
            $"+{quantity.ToString()}";
    }
}