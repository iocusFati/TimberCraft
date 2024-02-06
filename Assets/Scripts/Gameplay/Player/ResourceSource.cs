using System.Collections;
using System.Collections.Generic;
using Infrastructure.Services.Pool;
using UnityEngine;

namespace Infrastructure.States
{
    public abstract class ResourceSource : MonoBehaviour
    {
        [SerializeField] private Transform _hitParticleAppearAt;
        [SerializeField] protected List<Transform> _segments;

        protected float _restoreSourceAfter;
        protected BasePool<DropoutResource> _dropoutPool;
        protected BasePool<ParticleSystem> _particlePool;

        protected abstract void RemoveFirstStage();

        public virtual void GetDamage(Vector3 hitPoint, Transform hitTransform, out bool resourceSourceDestroyed)
        {
            PlayHitParticle(hitPoint, hitTransform);

            RemoveFirstStage();

            if (_segments.Count == 0)
            {
                OnLastStageDestroyed();
                resourceSourceDestroyed = true;
            }
            else
            {
                resourceSourceDestroyed = false;
            }

            ExtractDropouts();
        }

        protected virtual void ExtractDropouts()
        {
            DropoutResource dropout = _dropoutPool.Get();
            dropout.transform.position = _hitParticleAppearAt.position;
            
            dropout.SetTargetPositionFor_MMF_Feedback(transform.position);
            
            dropout.FeedbackPlayer.PlayFeedbacks();
        }

        protected virtual void PlayHitParticle(Vector3 hitPoint, Transform hitTransform)
        {
            ParticleSystem particle = _particlePool.Get();
            Vector3 position = _hitParticleAppearAt.position;
            
            particle.transform.position = new Vector3(position.x, hitPoint.y, position.z);
            particle.transform.rotation = hitTransform.rotation;

        }

        protected virtual void OnLastStageDestroyed()
        {
            StartCoroutine(WaitAndRestoreSource());
        }

        protected virtual void RestoreSource()
        {
            
        }

        private IEnumerator WaitAndRestoreSource()
        {
            yield return new WaitForSeconds(_restoreSourceAfter);
            
            RestoreSource();
        }
    }
}