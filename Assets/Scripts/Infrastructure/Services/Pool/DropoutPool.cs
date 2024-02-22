using System;
using Gameplay.Resource;
using Infrastructure.AssetProviderService;
using Infrastructure.States;
using UnityEngine;
using Utils;

namespace Infrastructure.Services.Pool
{
    public class DropoutPool : BasePool<DropoutResource>
    {
        private string _pathToAsset;

        public DropoutPool(IAssets assets, ResourceType dropoutType) : base(assets)
        {
            switch (dropoutType)
            {
                case ResourceType.Wood:
                    _pathToAsset = AssetPaths.LogPrefab;
                    break;
                case ResourceType.Stone:
                    _pathToAsset = AssetPaths.StoneBlockPrefab;
                    break;
                case ResourceType.Gold:
                    break;
                case ResourceType.Coin:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dropoutType), dropoutType, null);
            }
        }

        protected override DropoutResource Spawn()
        {
            DropoutResource dropoutResource = _assets.InstantiateDI<DropoutResource>(_pathToAsset);
            dropoutResource.Destination.SetParent(null);

            dropoutResource.SetReleaseDelegate(() => Release(dropoutResource));
            
            return dropoutResource;
        }
    }
}