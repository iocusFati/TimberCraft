using System.Collections;
using DG.Tweening;
using Gameplay.Lumberjack;
using Gameplay.Resource;
using Infrastructure;
using Infrastructure.StaticData.ResourcesData;
using UnityEngine;

namespace Gameplay.Bots.StateMachine.States
{
    public class LumberjackBotStorageResourceShare
    {
        private readonly LumberjackBotStorage _lumberjackBotStorage;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IGameResourceStorage _gameResourceStorage;

        private readonly float _deliverResourceDuration;
        private readonly float _timeGapBetweenResourcesDelivery;

        public LumberjackBotStorageResourceShare(LumberjackBotStorage lumberjackBotStorage, ICoroutineRunner coroutineRunner,
            ResourcesConfig resourcesConfig, IGameResourceStorage gameResourceStorage)
        {
            _lumberjackBotStorage = lumberjackBotStorage;
            _coroutineRunner = coroutineRunner;
            _gameResourceStorage = gameResourceStorage;

            _deliverResourceDuration = resourcesConfig.DeliverResourceDuration;
            _timeGapBetweenResourcesDelivery = resourcesConfig.TimeGapBetweenResourcesDelivery;
        }

        public void ShareAllResources(IResourceBuildingReceivable shareWithBuilding)
        {
            DropoutResource dropout = _lumberjackBotStorage.ResourceDropouts.Peek();
            
            _lumberjackBotStorage.LeaveResources();
            _gameResourceStorage.TakeResource(dropout.Type,
                _lumberjackBotStorage.ResourceDropouts.Count * dropout.ResourceValue);

            _coroutineRunner.StartCoroutine(ShareResourcesAnimation(shareWithBuilding));
        }

        private IEnumerator ShareResourcesAnimation(IResourceBuildingReceivable shareWithBuilding)
        {
            int resourcesCount = _lumberjackBotStorage.ResourceDropouts.Count;
            
            for (int index = 0; index < resourcesCount; index++)
            {
                DropoutResource resource = _lumberjackBotStorage.ResourceDropouts.Pop();

                resource.transform
                    .DOMove(shareWithBuilding.ReceiveResourceTransform.position, _deliverResourceDuration)
                    .OnComplete(() => OnResourceDelivered(resource));

                yield return new WaitForSeconds(_timeGapBetweenResourcesDelivery);
            }
        }

        private void OnResourceDelivered(DropoutResource resource)
        {
            resource.Release();
        }
    }
}