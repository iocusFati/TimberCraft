using Infrastructure.AssetProviderService;
using Infrastructure.StaticData.PlayerData;
using Infrastructure.StaticData.ResourcesData;
using UnityEngine;

namespace Infrastructure.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        public PlayerConfig PlayerConfig { get; set; }
        public ResourcesConfig ResourcesConfig { get; set; }

        public void Initialize()
        {
            InitializePlayerConfig();
            InitializeResourcesConfig();
        }

        private void InitializePlayerConfig() => 
            PlayerConfig = Resources.Load<PlayerConfig>(AssetPaths.PlayerConfig);
        
        private void InitializeResourcesConfig() => 
            ResourcesConfig = Resources.Load<ResourcesConfig>(AssetPaths.ResourcesConfig);
    }
}