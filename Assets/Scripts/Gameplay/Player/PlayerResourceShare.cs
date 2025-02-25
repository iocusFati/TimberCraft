using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Environment.Buildings;
using Gameplay.Resource;
using Gameplay.Resource.ResourceStorage;
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

        private readonly int _resourcesInOneDropout;
        private readonly float _cooldown;

        private readonly Transform _resourcesSpawnTransform;

        private bool _canShareCooldown = true;
        private PositivityEnum _currentAnimationDirection = PositivityEnum.Negative;

        private readonly Dictionary<ResourceType, DropoutPool> _dropoutsPool;


        public PlayerResourceShare(
            IGameResourceStorage gameResourceStorage, 
            Transform resourcesSpawnTransform,
            PlayerConfig playerConfig, 
            IPoolService poolService)
        {
            _gameResourceStorage = gameResourceStorage;
            _resourcesSpawnTransform = resourcesSpawnTransform;

            _resourcesInOneDropout = playerConfig.ResourcesInOneDropout;
            _cooldown = playerConfig.ResourceDeliveryCooldown;

            _dropoutsPool = poolService.DropoutsPool;
        }

        public async UniTask ShareResourcesForConstructionWith(IResourceBuildingReceivable receivable) => 
            await ShareResourcesWith(receivable, true);

        public async UniTask<bool> ShareResourcesWith(IResourceBuildingReceivable receivable, bool isForConstruction,
            int? specifiedResourceCount = null,
            Action onResourceDelivered = null)
        {
            int neededResources = specifiedResourceCount ?? receivable.NeededResources;
            ResourceType resourceType = receivable.ConstructionResourceType;
            
            if (!CanShare(neededResources, resourceType, isForConstruction, out int shareQuantity))
                return false;

            DropoutResource dropoutResource = _dropoutsPool[resourceType].Get();

            receivable.PromiseResource(shareQuantity);


            AnimateShare(receivable, dropoutResource, () =>
            {
                OnResourceDelivered(receivable, isForConstruction, dropoutResource, shareQuantity);
                onResourceDelivered?.Invoke();
            })
                .Forget();
            
            await WaitForCooldown();

            return true;
        }

        private void OnResourceDelivered(IResourceBuildingReceivable receivable, bool isForConstruction,
            DropoutResource dropoutResource, int resourceShareQuantity)
        {
            dropoutResource.Release();

            _gameResourceStorage.TryGiveResource(dropoutResource.Type, resourceShareQuantity);

            if (isForConstruction) 
                receivable.ReceiveResource();
        }

        private bool CanShare(int neededResources, ResourceType resourceType, bool isForConstruction,
            out int resourceShareQuantity)
        {
            return (_canShareCooldown && neededResources >= 0) & 
                   ResourcesExistInResourceHolder(neededResources, resourceType, isForConstruction, out resourceShareQuantity);
        }

        private async UniTask AnimateShare(IResourceBuildingReceivable receivable, DropoutResource resource,
            Action onFinishedAnimation)
        {
            resource.transform.position = _resourcesSpawnTransform.position;

            _currentAnimationDirection = GetAnimationDirection();
            
            await resource.PlayShareAnimationTo(receivable.ReceiveResourceTransform.position, _currentAnimationDirection);
            
            onFinishedAnimation.Invoke();
        }

        private PositivityEnum GetAnimationDirection() =>
            (PositivityEnum)((int)_currentAnimationDirection < 2 
                ? (int)_currentAnimationDirection + 1 
                : -1);

        private bool ResourcesExistInResourceHolder(int shareQuantity, ResourceType resourceType,
            bool isForConstruction, out int resourceShareQuantity)
        {
            int existingResources = _gameResourceStorage.GetResourceCountOfType(resourceType);
            
            resourceShareQuantity = shareQuantity < _resourcesInOneDropout 
                ? shareQuantity
                : _resourcesInOneDropout;

            if (isForConstruction && existingResources < resourceShareQuantity && existingResources > 0)
                resourceShareQuantity = existingResources;
            
            if (existingResources <= 0)
                return false;
            
            return true;
        }

        private async UniTask WaitForCooldown()
        {
            _canShareCooldown = false;

            await Task.Delay(TimeSpan.FromSeconds(_cooldown));

            _canShareCooldown = true;
        }
    }
}