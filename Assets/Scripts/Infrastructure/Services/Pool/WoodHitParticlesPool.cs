using Infrastructure.AssetProviderService;
using UnityEngine;
using Utils;

namespace Infrastructure.Services.Pool
{
    public class WoodHitParticlesPool : BasePool<ParticleSystem>
    {
        public WoodHitParticlesPool(IAssets assets) : base(assets)
        {
        }

        protected override ParticleSystem Spawn() => 
            _assets.Instantiate<ParticleSystem>(AssetPaths.WoodHitParticle);
    }
}