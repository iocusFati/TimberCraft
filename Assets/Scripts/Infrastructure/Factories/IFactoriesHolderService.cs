using Infrastructure.Factories.BotFactoryFolder;
using Infrastructure.Factories.PlayerFactoryFolder;
using Infrastructure.Services;

namespace Infrastructure.Factories
{
    public interface IFactoriesHolderService : IService
    {
        PlayerFactory PlayerFactory { get; }
        BotFactory BotFactory { get; }
    }
}