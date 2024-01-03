using Infrastructure.Services;
using Infrastructure.States;

namespace Infrastructure.Factories
{
    public interface IFactoriesHolderService : IService
    {
        PlayerFactory PlayerFactory { get; }
        LocationFactory LocationFactory { get; }
    }
}