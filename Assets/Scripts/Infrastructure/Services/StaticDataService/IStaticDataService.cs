using Infrastructure.StaticData.LumberjackData;
using Infrastructure.StaticData.ResourcesData;
using Infrastructure.StaticData.uiData;

namespace Infrastructure.Services.StaticDataService
{
    public interface IStaticDataService : IService
    {
        PlayerConfig PlayerConfig { get; }
        ResourcesConfig ResourcesConfig { get; }
        LumberjackBotConfig LumberjackBotConfig { get; }
        UIConfig UIConfig { get; }
    }
}