using Gameplay.Bots;
using Gameplay.Locations;
using Infrastructure.AssetProviderService;
using Infrastructure.States;
using UnityEngine;
using Utils;
using Zenject;

namespace Infrastructure.Factories.BotFactoryFolder
{
    public class BotFactory
    {
        private readonly IInstantiator _instantiator;

        public BotFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public void CreateLumberjackBotFrom(MinionHut minionHut, ResourceSourcesHolder resourceSourcesHolder)
        {
            LumberjackBot lumberjackBot = _instantiator
                .InstantiatePrefabResourceForComponent<LumberjackBot>(AssetPaths.LumberjackBot,
                    minionHut.TriggerZoneTransform.position, Quaternion.identity, new GameObject("Holder").transform);

            lumberjackBot.Initialize(resourceSourcesHolder, minionHut);

            lumberjackBot.transform.SetParent(null);
        }
    }
}