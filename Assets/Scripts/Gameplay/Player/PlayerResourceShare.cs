using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Buildings;
using Gameplay.Resource;
using Gameplay.Resource.ResourceStorage;
using Infrastructure;
using Infrastructure.Services.Pool;
using Infrastructure.StaticData.LumberjackData;
using MoreMountains.Feedbacks;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace Gameplay.Player
{
    public class PlayerResourceShare
    {
        private readonly IGameResourceStorage _gameResourceStorage;
        private readonly ICoroutineRunner _coroutineRunner;

        private readonly int _resourcesInOneDropout;
        private readonly float _deliverResourceDuration;
        private readonly float _cooldown;

        private readonly Transform _resourcesSpawnTransform;

        private bool _canShare = true;
        private PositivityEnum _currentAnimationDirection = PositivityEnum.Negative;

        private readonly Dictionary<ResourceType, DropoutPool> _dropoutsPool;


        public PlayerResourceShare(
            IGameResourceStorage gameResourceStorage, 
            Transform resourcesSpawnTransform,
            PlayerConfig playerConfig, 
            IPoolService poolService, ICoroutineRunner coroutineRunner)
        {
            _gameResourceStorage = gameResourceStorage;
            _resourcesSpawnTransform = resourcesSpawnTransform;
            _coroutineRunner = coroutineRunner;

            _resourcesInOneDropout = playerConfig.ResourcesInOneDropout;
            _deliverResourceDuration = playerConfig.DeliverResourceDuration;
            _cooldown = playerConfig.ResourceDeliveryCooldown;

            _dropoutsPool = poolService.DropoutsPool;
        }

        public async Task ShareResourcesForConstructionWith(IResourceBuildingReceivable receivable) => 
            await ShareResourcesWith(receivable, true);

        public async Task<bool> ShareResourcesWith(IResourceBuildingReceivable receivable, bool isForConstruction,
            int? specifiedResourceCount = null)
        {
            int neededResources = specifiedResourceCount ?? receivable.NeededResources;
            ResourceType resourceType = receivable.ConstructionResourceType;

            if (!_canShare || neededResources <= 0)
                return false;

            if (!TryShareResource(receivable, neededResources, resourceType, isForConstruction))
                return false;
            
            AnimateShare(receivable, resourceType);

            await WaitForCooldown();

            return true;
        }

        private void OnResourceDelivered(DropoutResource resource)
        {
            resource.Release();
        }

        private void AnimateShare(IResourceBuildingReceivable receivable, ResourceType resourceType)
        {
            DropoutResource resource = _dropoutsPool[resourceType].Get();
            resource.transform.position = _resourcesSpawnTransform.position;

            _currentAnimationDirection = (PositivityEnum)((int)_currentAnimationDirection * -1);
            
            resource.PlayShareAnimationTo(receivable.ReceiveResourceTransform.position, _currentAnimationDirection,
                OnResourceDelivered);
        }

        private bool TryShareResource(IResourceBuildingReceivable receivable, int shareQuantity,
            ResourceType resourceType, bool shouldReceive)
        {
            int resourceShareQuantity = shareQuantity < _resourcesInOneDropout 
                ? shareQuantity
                : _resourcesInOneDropout;

            if (!_gameResourceStorage.TryGiveResource(resourceType, resourceShareQuantity))
                return false;

            if (shouldReceive) 
                receivable.ReceiveResource(resourceShareQuantity);

            return true;
        }

        private async Task WaitForCooldown()
        {
            _canShare = false;

            await Task.Delay((int)(_cooldown * 1000));

            _canShare = true;
        }
    }
}