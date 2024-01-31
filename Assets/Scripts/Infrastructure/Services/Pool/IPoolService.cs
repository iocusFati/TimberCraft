namespace Infrastructure.Services.Pool
{
    public interface IPoolService : IService
    {
        WoodHitParticlesPool WoodHitParticlesPool { get; }
    }
}