using System.Collections.Generic;
using Infrastructure.Services.Pool;
using Infrastructure.Services.StaticDataService;
using UnityEngine;
using Zenject;

namespace Gameplay.Resource.StoneFolder
{
    public class StoneSource : ResourceSource
    {
        [SerializeField] private List<Transform> _particlePositions;

        [Inject]
        public void Construct(IPoolService poolService, IStaticDataService staticData)
        {
            _particlePool = poolService.StoneHitParticlesPool;
            _logPool = poolService.DropoutsPool[ResourceType.Stone];
            
            _restoreSourceAfter = staticData.ResourcesConfig.RestoreTreeAfter;
        }

        protected override void PlayHitParticle(Vector3 hitPoint, Transform hitTransform)
        {
            _hitParticleAppearAt.position = _particlePositions[^1].position;
            
            base.PlayHitParticle(hitPoint, hitTransform);
        }

        protected override void RestoreSource()
        {
            base.RestoreSource();

            foreach (var segment in _segmentsCopy)
            {
                segment.gameObject.SetActive(true);
                _segments.Add(segment);
            }
        }
    }
}