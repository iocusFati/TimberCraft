using Infrastructure.StaticData.LumberjackData;
using Infrastructure.StaticData.ResourcesData;

namespace Infrastructure.Services.StaticDataService
{
    public interface IStaticDataService : IService
    {
        PlayerConfig PlayerConfig { get; }
        ResourcesConfig ResourcesConfig { get; }
        LumberjackBotConfig LumberjackBotConfig { get; }
    }
}