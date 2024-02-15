using Gameplay.Resource;
using Infrastructure.AssetProviderService;
using Infrastructure.States;
using UnityEngine;
using Utils;

namespace Infrastructure.Services.Pool
{
    public class DropoutPool : BasePool<DropoutResource>
    {
        public DropoutPool(IAssets assets) : base(assets)
        {
        }

        protected override DropoutResource Spawn()
        {
            DropoutResource dropoutResource = _assets.InstantiateDI<DropoutResource>(AssetPaths.LogPrefab);
            dropoutResource.Destination.SetParent(null);

            dropoutResource.SetReleaseDelegate(() => Release(dropoutResource));
            
            return dropoutResource;
        }
    }
}