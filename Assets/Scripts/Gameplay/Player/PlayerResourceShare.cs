using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Bots.StateMachine.States;
using Gameplay.Resource;
using Infrastructure;
using Infrastructure.Services.Pool;
using Infrastructure.StaticData.LumberjackData;
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
            if (!_canShare)
                return;
            
            ResourceType resourceType = receivable.ConstructionResourceType;

            if (!_gameResourceStorage.TryGiveResource(resourceType, _resourcesInOneDropout))
                return;

            DropoutResource resource = _dropoutsPool[resourceType].Get();
            resource.transform.position = _resourcesSpawnTransform.position;
            
            receivable.ReceiveResource(_resourcesInOneDropout);
            
            resource.transform
                .DOMove(receivable.ReceiveResourceTransform.position, _deliverResourceDuration)
                .OnComplete(() => OnResourceDelivered(resource, receivable));

            _coroutineRunner.StartCoroutine(WaitForCooldown());
        }

        private void OnResourceDelivered(DropoutResource resource, IResourceBuildingReceivable receivable)
        {
            resource.Release();
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