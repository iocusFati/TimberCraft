using Infrastructure.StaticData.BuildingsData;
using Infrastructure.StaticData.LumberjackData;
using Infrastructure.StaticData.ResourcesData;
using Infrastructure.StaticData.uiData;
using UnityEngine;
using Utils;

namespace Infrastructure.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        public PlayerConfig PlayerConfig { get; private set; }
        public ResourcesConfig ResourcesConfig { get; private set; }
        public LumberjackBotConfig LumberjackBotConfig { get; private set; }
        public UIConfig UIConfig { get; private set; }
        public BuildingsConfig BuildingsConfig { get; private set; }
        public MinionHutUpgradeData MinionHutUpgradeData { get; private set; }
        public MainHouseUpgradeData MainHouseUpgradeData { get; private set; }


        public void Initialize()
        {
            InitializePlayerConfig();
            InitializeResourcesConfig();
            InitializeLumberjackBotConfig();
            InitializeUIConfig();
            InitializeBuildingsConfig();
            InitializeMinionHutUpgradeData();
            InitializeMainHouseUpgradeData();
        }

        private void InitializePlayerConfig() => 
            PlayerConfig = Resources.Load<PlayerConfig>(AssetPaths.PlayerConfig);
        
        private void InitializeResourcesConfig() => 
            ResourcesConfig = Resources.Load<ResourcesConfig>(AssetPaths.ResourcesConfig);
        
        private void InitializeLumberjackBotConfig() => 
            LumberjackBotConfig = Resources.Load<LumberjackBotConfig>(AssetPaths.LumberjackBotConfig);
        
        private void InitializeUIConfig() => 
            UIConfig = Resources.Load<UIConfig>(AssetPaths.UIConfig);
        
        private void InitializeBuildingsConfig() => 
            BuildingsConfig = Resources.Load<BuildingsConfig>(AssetPaths.BuildingsConfig);
        
        private void InitializeMinionHutUpgradeData() => 
            MinionHutUpgradeData = Resources.Load<MinionHutUpgradeData>(AssetPaths.MinionHutUpgradeData);
        
        private void InitializeMainHouseUpgradeData() => 
            MainHouseUpgradeData = Resources.Load<MainHouseUpgradeData>(AssetPaths.MainHouseUpgradeData);
    }
}