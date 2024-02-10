using Gameplay.Resource;
using Infrastructure.States;

namespace Infrastructure.Services.Pool
{
    public interface IPoolService : IService
    {
        WoodHitParticlesPool WoodHitParticlesPool { get; }
        BasePool<DropoutResource> LogsPool { get; }
    }
}