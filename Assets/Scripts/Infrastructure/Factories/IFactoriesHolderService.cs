using Infrastructure.Factories.BotFactoryFolder;
using Infrastructure.Factories.Location;
using Infrastructure.Factories.PlayerFactoryFolder;
using Infrastructure.Services;
using Infrastructure.States;

namespace Infrastructure.Factories
{
    public interface IFactoriesHolderService : IService
    {
        PlayerFactory PlayerFactory { get; }
        LocationFactory LocationFactory { get; }
        BotFactory BotFactory { get; }
    }
}