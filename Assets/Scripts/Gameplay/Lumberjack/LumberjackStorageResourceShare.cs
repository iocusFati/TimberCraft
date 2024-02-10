using System.Collections;
using DG.Tweening;
using Gameplay.Lumberjack;
using Gameplay.Resource;
using Infrastructure;
using Infrastructure.StaticData.ResourcesData;
using UnityEngine;

namespace Gameplay.Bots.StateMachine.States
{
    public class LumberjackStorageResourceShare
    {
        private readonly LumberjackStorage _lumberjackStorage;
        private readonly ICoroutineRunner _coroutineRunner;
        
        private readonly float _deliverResourceDuration;
        private readonly float _timeGapBetweenResourcesDelivery;

        public LumberjackStorageResourceShare(LumberjackStorage lumberjackStorage, ICoroutineRunner coroutineRunner,
            ResourcesConfig resourcesConfig)
        {
            _lumberjackStorage = lumberjackStorage;
            _coroutineRunner = coroutineRunner;

            _deliverResourceDuration = resourcesConfig.DeliverResourceDuration;
            _timeGapBetweenResourcesDelivery = resourcesConfig.TimeGapBetweenResourcesDelivery;
        }

        public void ShareAllResources(IResourceBuildingReceivable shareWithBuilding)
        {
            _lumberjackStorage.LeaveResources();

            _coroutineRunner.StartCoroutine(ShareResourcesAnimation(shareWithBuilding));
        }

        private IEnumerator ShareResourcesAnimation(IResourceBuildingReceivable shareWithBuilding)
        {
            int resourcesCount = _lumberjackStorage.ResourceDropouts.Count;
            
            for (int index = 0; index < resourcesCount; index++)
            {
                DropoutResource resource = _lumberjackStorage.ResourceDropouts.Pop();

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