using System.Collections;
using System.Collections.Generic;
using Gameplay.Buildings;
using Gameplay.Resource;
using Gameplay.Resource.ResourceStorage;
using Infrastructure;
using Infrastructure.Services.Pool;
using Infrastructure.StaticData.LumberjackData;
using MoreMountains.Feedbacks;
using UnityEngine;

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

        public void ShareForConstructionWith(IResourceBuildingReceivable receivable)
        {
            if (!_canShare || receivable.NeededResources <= 0)
                return;
            
            ResourceType resourceType = receivable.ConstructionResourceType;

            if (!TryShareResourceForConstruction(receivable, resourceType))
                return;
            
            AnimateShare(receivable, resourceType);
            
            _coroutineRunner.StartCoroutine(WaitForCooldown());
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

        private bool TryShareResourceForConstruction(IResourceBuildingReceivable receivable, ResourceType resourceType)
        {
            int resourceShareQuantity = receivable.NeededResources < _resourcesInOneDropout 
                ? receivable.NeededResources
                : _resourcesInOneDropout;

            if (!_gameResourceStorage.TryGiveResource(resourceType, resourceShareQuantity))
                return false;
            
            receivable.ReceiveResource(resourceShareQuantity);

            return true;
        }

        private IEnumerator WaitForCooldown()
        {
            _canShare = false;
            
            yield return new WaitForSeconds(_cooldown);

            _canShare = true;
            Debug.Log(_canShare);
        }
    }
}