using Gameplay.Bots.StateMachine.States;
using Gameplay.Resource;
using Infrastructure.Factories;
using Infrastructure.Factories.BotFactoryFolder;
using UnityEngine;
using Zenject;

namespace Gameplay.Locations
{
    public class MinionHut : MonoBehaviour, IResourceBuildingReceivable
    {
        public Transform TriggerZoneTransform;
        public ResourceType BotResourceType;
        
        [SerializeField] private Transform _receiveResourceTransform;

        private BotFactory _botFactory;

        public Transform ReceiveResourceTransform => _receiveResourceTransform;

        [Inject]
        public void Construct(IFactoriesHolderService factoriesHolder)
        {
            _botFactory = factoriesHolder.BotFactory;
        }

        public void SpawnBots(ResourceSourcesHolder resourceSourcesHolder)
        {
            _botFactory.CreateLumberjackBotFrom(this, resourceSourcesHolder);
        }
    }
}