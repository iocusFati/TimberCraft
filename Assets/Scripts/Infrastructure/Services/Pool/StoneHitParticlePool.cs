using Infrastructure.AssetProviderService;
using UnityEngine;
using Utils;

namespace Infrastructure.Services.Pool
{
    public class StoneHitParticlePool : BasePool<ParticleSystem>

    {
        public StoneHitParticlePool(IAssets assets) : base(assets)
        {
        }

        protected override ParticleSystem Spawn() => 
            _assets.Instantiate<ParticleSystem>(AssetPaths.StoneHitParticle);
    }
}