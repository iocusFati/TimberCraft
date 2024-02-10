using Gameplay.Resource;
using Infrastructure.AssetProviderService;
using Infrastructure.States;
using UnityEngine;

namespace Infrastructure.Services.Pool
{
    public class LogsPool : BasePool<DropoutResource>
    {
        public LogsPool(IAssets assets) : base(assets)
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