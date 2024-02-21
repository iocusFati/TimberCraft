using Gameplay.Resource;
using Infrastructure.Services.Cache;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Lumberjack
{
    public class LumberjackAxe : MonoBehaviour
    {
        private CacheContainer<ResourceSource> _resourceSourcesCache;
        
        private bool _disableHitCheck = true;

        [Inject]
        public void Construct(ICacheService cacheService)
        {
            _resourceSourcesCache = cacheService.ResourceSources;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!_disableHitCheck && other.CompareTag(Tags.Resource))
            {
                ResourceSource resourceSource = _resourceSourcesCache.Get(other.gameObject);
                resourceSource.GetDamage(hitPoint: other.ClosestPoint(transform.position), transform, out _);
            }
        }

        public void DisableHitCheck(bool disable)
        {
            _disableHitCheck = disable;
            Debug.Log("Hit check: " + _disableHitCheck);
        }
    }
}