using Infrastructure.AssetProviderService;
using Infrastructure.Services.Pool.Particle;
using UnityEngine;
using Utils;

namespace Infrastructure.Services.Pool
{
    public class ParticlePool : BasePool<ParticleSystem>
    {
        private readonly string _particlePath;

        public ParticlePool(IAssets assets, string particlePath) : base(assets)
        {
            _particlePath = particlePath;
        }

        protected override ParticleSystem Spawn()
        {
            ParticleComplete particleComplete = _assets.Instantiate<ParticleComplete>(_particlePath);
            ParticleSystem particleSystem = particleComplete.GetComponent<ParticleSystem>();

            particleComplete.SetRelease(() => Release(particleSystem));

            return particleSystem;
        }
    }
}