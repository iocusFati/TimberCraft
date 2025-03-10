﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay.Resource.StoneFolder;
using Infrastructure.Services.Pool;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.ResourcesData;
using MoreMountains.Feedbacks;
using UnityEngine;
using Zenject;

namespace Gameplay.Resource
{
    public abstract class ResourceSource : MonoBehaviour
    {
        [SerializeField] private MMF_Player _appearFeedback;
        [SerializeField] private Collider _сollider;
        [SerializeField] protected Transform _hitParticleAppearAt;
        [SerializeField] protected List<Transform> _segments;

        protected float _restoreSourceAfter;
        private int _resourcesValue;
        protected ResourcesConfig _resourcesConfig;

        protected BasePool<DropoutResource> _logPool;
        protected ParticlePool _hitParticlePool;

        protected Transform[] _segmentsCopy;
        // protected Dictionary<GameObject,Collider> _segmentColliders;

        public ResourceSourceState CurrentState { get; private set; } = ResourceSourceState.Untouched;
        public ResourceType Type { get; private set; }
        protected Transform[] InactiveSegments => _segmentsCopy.Except(_segments).ToArray();

        public event Action<ResourceSource> OnResourceMined;

        [Inject]
        public void Construct(IStaticDataService staticData)
        {
            _resourcesConfig = staticData.ResourcesConfig;
        }
        
        public virtual void Construct(int resourcesValue)
        {
            _resourcesValue = resourcesValue;

            _segmentsCopy = _segments.ToArray();
            
            ExtractSegmentColliders();
        }

        protected virtual void Awake()
        {
            SetResourceType();
        }

        public virtual async UniTask<bool> GetDamage(Vector3 hitPoint, Transform hitTransform)
        {
            await WaitForStageDestroyAsync();
            
            PlayHitParticle(hitPoint, hitTransform);
            DestroyStage();
            ExtractDropouts();
            
            if (_segments.Count == 0)
            {
                OnLastStageDestroyed();
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual UniTask WaitForStageDestroyAsync() => 
            UniTask.CompletedTask;

        public virtual void StartMining() => 
            CurrentState = ResourceSourceState.BeingMined;

        public virtual void StopMining() => 
            CurrentState = ResourceSourceState.Untouched;

        public bool CanBeMinedByBotWithType(ResourceType botTargetResourceType) =>
            Type == botTargetResourceType || botTargetResourceType == ResourceType.All;

        protected virtual void ExtractDropouts()
        {
            for (int i = 0; i < _resourcesConfig.RandomDropoutsPerExtract; i++)
            {
                DropoutResource dropout = _logPool.Get();
                dropout.transform.position = _hitParticleAppearAt.position;
                dropout.ResourceValue = _resourcesValue;

                dropout.SetTargetPositionFor_MMF_Feedback(transform.position);
                dropout.FeedbackPlayer.PlayFeedbacks();
                
                dropout.SetReleaseTimer().Forget();
            }
        }

        protected virtual void PlayHitParticle(Vector3 hitPoint, Transform hitTransform)
        {
            Vector3 position = _hitParticleAppearAt.position;
            ParticleSystem particle = _hitParticlePool.Get(new Vector3(position.x, hitPoint.y, position.z));
            
            particle.transform.rotation = hitTransform.rotation;

        }

        protected virtual void OnLastStageDestroyed()
        {
            StartCoroutine(WaitAndRestoreSource());

            _сollider.enabled = false;

            CurrentState = ResourceSourceState.Mined;
            
            OnResourceMined?.Invoke(this);
        }

        protected virtual void RestoreSource()
        {
            CurrentState = ResourceSourceState.Untouched;
            
            _сollider.enabled = true;

            _appearFeedback.PlayFeedbacks();
        }

        protected virtual void DestroyStage()
        {
            _segments[0].gameObject.SetActive(false);
            _segments.RemoveAt(0);
        }

        private void ExtractSegmentColliders()
        {
            Collider[] segmentColliders = _segments
                .Select(segment => segment.GetComponent<Collider>())
                .ToArray();
            
            // _segmentColliders = segmentColliders.ToDictionary(
            //     keySelector => keySelector.gameObject, 
            //     segmentCollider => segmentCollider);
        }

        private void SetResourceType()
        {
            Type = this switch
            {
                Tree => ResourceType.Wood,
                StoneSource => ResourceType.Stone,
                _ => Type
            };
        }

        private IEnumerator WaitAndRestoreSource()
        {
            yield return new WaitForSeconds(_restoreSourceAfter);
            
            RestoreSource();
        }
    }
}