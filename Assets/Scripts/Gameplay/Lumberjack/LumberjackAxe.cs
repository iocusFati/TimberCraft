using System;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Lumberjack
{
    public class LumberjackAxe : MonoBehaviour
    {
        private bool _disableHitCheck = true;

        public event Action OnDestroyedResourceSource;

        private void OnTriggerEnter(Collider other)
        {
            if (!_disableHitCheck && other.CompareTag("Resource"))
            {
                other
                    .GetComponentInParent<ResourceSource>()
                    .GetDamage(hitPoint: other.ClosestPoint(transform.position), transform, out bool resourceSourceDestroyed);
                
                DisableHitCheck(true);

                if (resourceSourceDestroyed) 
                    OnDestroyedResourceSource.Invoke();
            }
        }

        public void DisableHitCheck(bool disable)
        {
            _disableHitCheck = disable;
        }
    }
}