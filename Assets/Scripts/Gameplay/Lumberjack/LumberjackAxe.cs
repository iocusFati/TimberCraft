using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Resource;
using Infrastructure.Services.Cache;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.LumberjackData;
using MoreMountains.Feedbacks;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Lumberjack
{
    public class LumberjackAxe : MonoBehaviour
    {
        private CacheContainer<ResourceSource> _resourceSourcesCache;

        private bool _disableHitCheck = true;

        private int _treeMaxDamagesPerSwing;
        private int _stoneMaxDamagesPerSwing;

        private ResourceType _targetResourceType = ResourceType.None;
        private Collider _collider;

        private readonly List<GameObject> _damagedSources = new();

        [Inject]
        public void Construct(ICacheService cacheService, IStaticDataService staticData)
        {
            _resourceSourcesCache = cacheService.ResourceSources;

            PlayerConfig playerConfig = staticData.PlayerConfig;
            _treeMaxDamagesPerSwing = playerConfig.TreeMaxDamagesPerSwing;
            _stoneMaxDamagesPerSwing = playerConfig.StoneMaxDamagesPerSwing;
        }

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private async void OnTriggerEnter(Collider other)
        {
            if (!_disableHitCheck && other.CompareTag(Tags.Resource))
            {
                ResourceSource resourceSource = _resourceSourcesCache.Get(other.gameObject);

                await TryDamageSource(other, resourceSource);
            }
        }

        public void SetTargetResourceType(ResourceType targetResourceType)
        {
            _targetResourceType = targetResourceType;
        }

        public void Recharge()
        {
            DisableHitCheck(true);
            _damagedSources.Clear();
        }

        public void DisableHitCheck(bool disable)
        {
            _disableHitCheck = disable;
            _collider.enabled = !disable;
        }

        private async UniTask TryDamageSource(Collider other, ResourceSource resourceSource)
        {
            if ((_targetResourceType == ResourceType.None ||
                 resourceSource.CanBeMinedByBotWithType(_targetResourceType)) &&
                !HaveBeenDamagedTooMuchTimes(resourceSource))
            {
                await resourceSource.GetDamage(hitPoint: other.ClosestPoint(transform.position), hitTransform: transform);
                
                _damagedSources.Add(resourceSource.gameObject);
            }
        }

        private bool HaveBeenDamagedTooMuchTimes(ResourceSource resourceSource)
        {
            int maxDamagesPerSwing;

            switch (resourceSource.Type)
            {
                case ResourceType.Wood:
                    maxDamagesPerSwing = _treeMaxDamagesPerSwing;
                    break;
                case ResourceType.Stone:
                    maxDamagesPerSwing = _stoneMaxDamagesPerSwing;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _damagedSources.Count(source => source.gameObject == resourceSource.gameObject) >= maxDamagesPerSwing;
        }
    }
}