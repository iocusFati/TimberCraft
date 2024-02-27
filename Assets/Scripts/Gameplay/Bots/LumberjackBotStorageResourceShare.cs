using System.Collections;
using Gameplay.Buildings;
using Gameplay.Lumberjack;
using Gameplay.Resource;
using Gameplay.Resource.ResourceStorage;
using Infrastructure;
using Infrastructure.StaticData.ResourcesData;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Gameplay.Bots
{
    public class LumberjackBotStorageResourceShare
    {
        private readonly LumberjackBotStorage _lumberjackBotStorage;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IGameResourceStorage _gameResourceStorage;

        private readonly float _deliverResourceDuration;
        private readonly float _timeGapBetweenResourcesDelivery;
        
        private bool _isSharing;
        private PositivityEnum _currentAnimationDirection = PositivityEnum.Negative;

        public LumberjackBotStorageResourceShare(LumberjackBotStorage lumberjackBotStorage,
            ICoroutineRunner coroutineRunner,
            ResourcesConfig resourcesConfig, 
            IGameResourceStorage gameResourceStorage)
        {
            _lumberjackBotStorage = lumberjackBotStorage;
            _coroutineRunner = coroutineRunner;
            _gameResourceStorage = gameResourceStorage;

            _deliverResourceDuration = resourcesConfig.DeliverResourceDuration;
            _timeGapBetweenResourcesDelivery = resourcesConfig.TimeGapBetweenResourcesDelivery;
        }

        public void ShareAllResources(IResourceBuildingReceivable shareWithBuilding)
        {
            if (_isSharing || _lumberjackBotStorage.ResourceDropouts.Count == 0)
                 return;
            
            DropoutResource dropout = _lumberjackBotStorage.ResourceDropouts.Peek();
            
            _lumberjackBotStorage.LeaveResources();
            _gameResourceStorage.TakeResource(dropout.Type,
                _lumberjackBotStorage.ResourceDropouts.Count * dropout.ResourceValue);

            _coroutineRunner.StartCoroutine(ShareResourcesAnimation(shareWithBuilding));
        }

        private IEnumerator ShareResourcesAnimation(IResourceBuildingReceivable shareWithBuilding)
        {
            _isSharing = true;
            int dropoutsCount = _lumberjackBotStorage.ResourceDropouts.Count;
            
            for (int index = 0; index < dropoutsCount; index++)
            {
                DropoutResource resource = _lumberjackBotStorage.ResourceDropouts.Pop();

                _currentAnimationDirection = (PositivityEnum)((int)_currentAnimationDirection * -1);

                resource.PlayShareAnimationTo(
                    shareWithBuilding.ReceiveResourceTransform.position, _currentAnimationDirection, OnResourceDelivered);

                yield return new WaitForSeconds(_timeGapBetweenResourcesDelivery);
            }

            _isSharing = false;
        }

        private void OnResourceDelivered(DropoutResource resource)
        {
            resource.Release();
        }
    }
}