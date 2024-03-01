using System;
using Gameplay.Resource;
using Gameplay.Resource.ResourceStorage;
using Infrastructure.Services.StaticDataService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Buttons
{
    public abstract class ButtonDisabler : MonoBehaviour
    {
        [SerializeField] protected ResourceType _resourceType;
        [SerializeField] private GameObject _buttonBlocker;

        private IGameResourceStorage _gameResourceStorage;

        private Button _sellResourceButton;

        protected abstract bool CanBuy(int newResourceCount);

        [Inject]
        public void Construct(
            IGameResourceStorage gameResourceStorage, 
            IResourcesSelling resourcesSelling,
            IStaticDataService staticData)
        {
            _gameResourceStorage = gameResourceStorage;
        }

        private void Awake()
        {
            _sellResourceButton = GetComponent<Button>();
            
            SubscribeToResourceCountChange();
            DisableButtonIfCantBuy(_gameResourceStorage.GetResourceCountOfType(_resourceType));
        }

        private void SubscribeToResourceCountChange()
        {
            switch (_resourceType)
            {
                case ResourceType.Wood:
                    _gameResourceStorage.OnWoodCountChanged += DisableButtonIfCantBuy; 
                    break;
                case ResourceType.Stone:
                    _gameResourceStorage.OnStoneCountChanged += DisableButtonIfCantBuy; 
                    break;
                case ResourceType.Gold:
                    _gameResourceStorage.OnGoldCountChanged += DisableButtonIfCantBuy; 
                    break;
                case ResourceType.Coin:
                    _gameResourceStorage.OnCoinCountChanged += DisableButtonIfCantBuy; 
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DisableButtonIfCantBuy(int newResourceCount)
        {
            SetButtonInteractable(CanBuy(newResourceCount));
        }

        private void SetButtonInteractable(bool interactable)
        {
            _sellResourceButton.interactable = interactable;
            _buttonBlocker.SetActive(!interactable);
        }
    }
}