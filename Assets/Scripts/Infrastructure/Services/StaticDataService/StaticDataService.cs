using Infrastructure.AssetProviderService;
using Infrastructure.StaticData.LumberjackData;
using Infrastructure.StaticData.ResourcesData;
using UnityEngine;

namespace Infrastructure.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        public PlayerConfig PlayerConfig { get; set; }
        public ResourcesConfig ResourcesConfig { get; private set; }
        public LumberjackBotConfig LumberjackBotConfig { get; set; }

        public void Initialize()
        {
            InitializePlayerConfig();
            InitializeResourcesConfig();
            InitializeLumberjackBotConfig();
        }

        private void InitializePlayerConfig() => 
            PlayerConfig = Resources.Load<PlayerConfig>(AssetPaths.PlayerConfig);
        
        private void InitializeResourcesConfig() => 
            ResourcesConfig = Resources.Load<ResourcesConfig>(AssetPaths.ResourcesConfig);
        
        private void InitializeLumberjackBotConfig() => 
            LumberjackBotConfig = Resources.Load<LumberjackBotConfig>(AssetPaths.LumberjackBotConfig);
    }
}