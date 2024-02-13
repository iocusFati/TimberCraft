using Gameplay.Bots.StateMachine.States;
using Gameplay.Resource;
using Infrastructure.Factories;
using Infrastructure.Factories.BotFactoryFolder;
using UnityEngine;
using Zenject;

namespace Gameplay.Locations
{
    public class MinionHut : Building
    {
        [Header("Minion hut")]
        public Transform TriggerZoneTransform;

        private BotFactory _botFactory;

        public override void InteractWithPlayer()
        {
            
        }

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