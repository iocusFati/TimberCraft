using Infrastructure.StaticData.PlayerData;
using Infrastructure.StaticData.ResourcesData;

namespace Infrastructure.Services.StaticDataService
{
    public interface IStaticDataService : IService
    {
        PlayerConfig PlayerConfig { get; set; }
        ResourcesConfig ResourcesConfig { get; }
    }
}