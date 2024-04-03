using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Gameplay.Environment.BirdAI
{
    public class HitAnotherCustomTrigger : MonoBehaviour, ICustomTrigger
    {
        [SerializeField] private Collider _another;
        [SerializeField] private LayerMask _collideWithLayer;

        private void Awake()
        {
        }

        public async UniTask WaitForTriggerAsync() =>
            await _another.OnCollisionEnterAsObservable()
                .Where(IsOfLayer)
                .ToUniTask(true);


        private bool IsOfLayer(Collision collision)
        {
            float log = Mathf.Log(_collideWithLayer.value, 2);
            return collision.gameObject.layer == log;
        }
    }
}