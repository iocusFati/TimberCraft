using System;
using Gameplay.Resource;
using Gameplay.Resource.ResourceStorage;
using Infrastructure.Services.StaticDataService;
using Sirenix.OdinInspector;
using TMPro;
using UI.Entities.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Entities.PopUps
{
    public class ResourcesShopPopUp : Window
    {
        [Title("Resources shop", titleAlignment: TitleAlignments.Centered)]
        [SerializeField] private Button _sellResourceButton;
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private Color _disableColor;

        [Header("Images")] 
        [SerializeField] private Image _resourceImage;
        [SerializeField] private Image _coinImage;
        
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI _resourceCountText;
        [SerializeField] private TextMeshProUGUI _coinsCountText;

        private IGameResourceStorage _gameResourceStorage;

        private int _resourceUnitsPerCoin;
        private ResourcesSelling _resourcesSelling;

        [Inject]
        public void Construct(IGameResourceStorage gameResourceStorage, IStaticDataService staticData)
        {
            _gameResourceStorage = gameResourceStorage;

            _resourceUnitsPerCoin = staticData.ResourcesConfig.ResourceUnitsPerCoin;

            _resourcesSelling = new ResourcesSelling(gameResourceStorage, staticData.ResourcesConfig);

            SubscribeToResourceCountChange();
        }

        private void Awake()
        {
            SetNewResourceCount();
            
            _sellResourceButton.onClick
                .AddListener(() =>_resourcesSelling.SellAllPossibleOfType(_resourceType));
        }

        private void SetNewResourceCount(int newResourceCount)
        {
            int sellResourceCount = _resourcesSelling.GetSellResourceCount(_resourceType, out int receiveCoins);

            if (sellResourceCount == 0)
            {
                SetButtonInteractable(false);
                sellResourceCount = _resourceUnitsPerCoin;
                receiveCoins = 1;
            }
            else
            {
                _sellResourceButton.interactable = true;
            }

            _resourceCountText.text = sellResourceCount.ToString();
            _coinsCountText.text = receiveCoins.ToString();
        }

        private void SetButtonInteractable(bool interactable)
        {
            _sellResourceButton.interactable = interactable;

            if (interactable) 
                SetButtonExtrasToColor(Color.white);
            else
            {
                SetButtonExtrasToColor(_disableColor);
            }

            void SetButtonExtrasToColor(Color color)
            {
                _resourceImage.color = color;
                _coinImage.color = color;
                // _coinsCountText.color = color;
                _resourceCountText.color = color;
            }
        }

        private void SetNewResourceCount()
        {
            switch (_resourceType)
            {
                case ResourceType.Wood:
                    SetNewResourceCount(_gameResourceStorage.WoodCount);
                    break;
                case ResourceType.Stone:
                    SetNewResourceCount(_gameResourceStorage.StoneCount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SubscribeToResourceCountChange()
        {
            switch (_resourceType)
            {
                case ResourceType.Wood:
                    _gameResourceStorage.OnWoodCountChanged += SetNewResourceCount; 
                    break;
                case ResourceType.Stone:
                    _gameResourceStorage.OnStoneCountChanged += SetNewResourceCount; 
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}