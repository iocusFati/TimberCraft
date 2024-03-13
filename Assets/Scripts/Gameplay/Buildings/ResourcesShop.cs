using System.Threading.Tasks;
using Gameplay.Player;
using Gameplay.Resource;
using Infrastructure.Factories;
using UI.Mediator;
using UnityEngine;
using Zenject;

namespace Gameplay.Buildings
{
    public class ResourcesShop : Building
    {
        [SerializeField] private ResourceType _resourceType;
        
        private IUIMediator _uiMediator;
        private PlayerResourceShare _playerResourceShare;
        private IResourcesSelling _resourcesSelling;
        
        private bool _stopInteractionWithPlayer;

        [Inject]
        public void Construct(IUIMediator uiMediator,
            IFactoriesHolderService factories,
            IResourcesSelling resourcesSelling)
        {
            _uiMediator = uiMediator;

            _resourcesSelling = resourcesSelling;

            factories.PlayerFactory.OnPlayerCreated += player => _playerResourceShare = player.ResourceShare;
        }

        public override async void InteractWithPlayer()
        {
            _stopInteractionWithPlayer = false;
            
            await BuyResourcesFromPlayer();
        }

        public override void StopInteractingWithPlayer()
        {
            _stopInteractionWithPlayer = true;
        }

        public async Task BuyResourcesFromPlayer()
        {
            while (!_stopInteractionWithPlayer)
            {
                await _resourcesSelling.TrySellToReceiveCoins(_resourceType, 1, this);
            }
        }
    }
}