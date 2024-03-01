using System.Collections.Generic;
using Infrastructure.Services.StaticDataService;
using TMPro;
using UI.Entities.PopUps;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.Mediator
{
    public class MainHousePopUp : UpgradablePopUp
    {
        [FormerlySerializedAs("_expandButton")] [SerializeField] private Button _unlockIslandButton;
        [FormerlySerializedAs("_priceText")] [SerializeField] private TextMeshProUGUI _costText;
        
        private List<int> _mainHouseUpgradeCosts;

        [Inject]
        public new void Construct(IStaticDataService staticData)
        {
            _mainHouseUpgradeCosts = staticData.MainHouseUpgradeData.LevelUpgradeCosts;
            _upgradeLevelsCount = _mainHouseUpgradeCosts.Count;
        }

        private void Awake()
        {
            _unlockIslandButton.onClick.AddListener(LevelUp);
        }

        protected override void OnFinalLevelReached()
        {
            _unlockIslandButton.gameObject.SetActive(false);
        }

        protected override void SetLevel(int level)
        {
            base.SetLevel(level);

            _costText.text = _mainHouseUpgradeCosts[level - 1].ToString();
        }

        private void Expand()
        {
            
        }
    }
}