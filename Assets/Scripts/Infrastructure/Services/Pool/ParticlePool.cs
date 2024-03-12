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
        
        public ParticleSystem Get(Vector3 stumpPosition)
        {
            ParticleSystem particleSystem = base.Get();

            particleSystem.transform.position = stumpPosition;

            ParticleRender particleRender = particleSystem.GetComponent<ParticleRender>();
            particleRender.DisableIfNotVisible();
            
            return particleSystem;
        }

        protected override ParticleSystem Spawn()
        {
            ParticleRender particleRender = _assets.Instantiate<ParticleRender>(_particlePath);
            ParticleSystem particleSystem = particleRender.GetComponent<ParticleSystem>();

            particleRender.SetRelease(() => Release(particleSystem));

            return particleSystem;
        }
    }
}