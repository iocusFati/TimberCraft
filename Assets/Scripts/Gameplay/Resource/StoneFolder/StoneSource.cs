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
        [SerializeField] private GameObject _stoneMark;

        [Inject]
        public void Construct(IPoolService poolService, IStaticDataService staticData)
        {
            _hitParticlePool = poolService.StoneHitParticlesPool;
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
            
            _stoneMark.SetActive(false);

            RestoreSegments();
        }

        protected override void OnLastStageDestroyed()
        {
            base.OnLastStageDestroyed();
            
            _stoneMark.SetActive(true);
        }

        protected override void DestroyStage()
        {
            _segmentColliders[_segments[0].gameObject].enabled = false;

            base.DestroyStage();
            
            if (_segments.Count > 0) 
                _segmentColliders[_segments[0].gameObject].enabled = true;
            else
                _segmentColliders[_segmentsCopy[0].gameObject].enabled = true;
        }

        private void RestoreSegments()
        {
            foreach (var segment in _segmentsCopy)
            {
                segment.gameObject.SetActive(true);
                _segments.Add(segment);
            }
        }
    }
}