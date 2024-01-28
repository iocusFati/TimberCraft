using Infrastructure.AssetProviderService;
using Infrastructure.StaticData.PlayerData;
using UnityEngine;

namespace Infrastructure.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        public PlayerConfig PlayerConfig { get; set; }

        public void Initialize()
        {
            InitializePlayerData();
        }

        private void InitializePlayerData()
        {
            PlayerConfig = Resources.Load<PlayerConfig>(AssetPaths.PlayerConfig);
        }
    }
}