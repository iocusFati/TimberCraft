using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Pool;
using Infrastructure.Services.StaticDataService;
using MoreMountains.Feedbacks;
using UnityEngine;
using Zenject;

namespace Gameplay.Resource.StoneFolder
{
    public class StoneSource : ResourceSource
    {
        [Header("Stone")]
        [SerializeField] private List<Transform> _particlePositions;
        [SerializeField] private GameObject _stoneMark;
        
        [Header("Feedbacks")]
        [SerializeField] private MMF_Player _damageFeedback;

        private MMF_Events _delayHitEvent;
        
        private readonly Dictionary<Transform, MMF_Flicker> _segmentFlickerPairs = new();

        [Inject]
        public void Construct(IPoolService poolService, IStaticDataService staticData)
        {
            _hitParticlePool = poolService.StoneHitParticlesPool;
            _logPool = poolService.DropoutsPool[ResourceType.Stone];
            
            _restoreSourceAfter = staticData.ResourcesConfig.RestoreSourceAfter;
        }

        protected override void Awake()
        {
            base.Awake();

            InitializeDamageFeedback();
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

        public override async UniTask<bool> GetDamage(Vector3 hitPoint, Transform hitTransform)
        {
            _damageFeedback.PlayFeedbacks();
            
            return await base.GetDamage(hitPoint, hitTransform);
        }

        protected override async UniTask WaitForStageDestroyAsync()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            
            await _delayHitEvent.PlayEvents.OnInvokeAsync(tokenSource.Token);
            
            tokenSource.Cancel();
        }

        protected override void OnLastStageDestroyed()
        {
            base.OnLastStageDestroyed();
            
            _stoneMark.SetActive(true);
        }

        protected override void DestroyStage()
        {
            _segmentColliders[_segments[0].gameObject].enabled = false;
            _segmentFlickerPairs[_segments[0]].Active = false;

            //first segment is removed from _segments here
            base.DestroyStage();
            
            if (_segments.Count > 0) 
                _segmentColliders[_segments[0].gameObject].enabled = true;
            else
                _segmentColliders[_segmentsCopy[0].gameObject].enabled = true;
        }

        private void InitializeDamageFeedback()
        {
            List<MMF_Flicker> flickers = _damageFeedback.GetFeedbacksOfType<MMF_Flicker>();

            for (var index = 0; index < flickers.Count; index++)
            {
                MMF_Flicker flicker = flickers[index];
                
                flicker.BoundRenderer = _segments[index].GetComponent<Renderer>();
                _segmentFlickerPairs.Add(_segments[index], flicker);
            }

            _delayHitEvent = _damageFeedback.GetFeedbackOfType<MMF_Events>();
        }

        private void RestoreSegments()
        {
            foreach (var segment in _segmentsCopy)
            {
                segment.gameObject.SetActive(true);
                _segments.Add(segment);
                
                _segmentFlickerPairs[segment].Active = true;
            }
        }
    }
}