using Infrastructure.StaticData.BuildingsData;
using Infrastructure.StaticData.LumberjackData;
using Infrastructure.StaticData.PoolData;
using Infrastructure.StaticData.ResourcesData;
using Infrastructure.StaticData.uiData;
using Infrastructure.StaticData.WindData;

namespace Infrastructure.Services.StaticDataService
{
    public interface IStaticDataService : IService
    {
        PlayerConfig PlayerConfig { get; }
        ResourcesConfig ResourcesConfig { get; }
        LumberjackBotConfig LumberjackBotConfig { get; }
        UIConfig UIConfig { get; }
        BuildingsConfig BuildingsConfig { get; }
        MinionHutUpgradeData MinionHutUpgradeData { get; }
        MainHouseUpgradeData MainHouseUpgradeData { get; }
        PoolConfig PoolConfig { get; }
        EnvironmentConfig EnvironmentConfig { get; }
    }
}