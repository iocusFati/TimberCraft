using UnityEngine;

namespace Infrastructure.States
{
    public class PlayerAxe : MonoBehaviour
    {
        private bool _disableHitCheck = true;

        private void OnTriggerEnter(Collider other)
        {
            if (!_disableHitCheck && other.CompareTag("Resource"))
            {
                other
                    .GetComponentInParent<ResourceSource>()
                    .GetDamage(hitPoint: other.ClosestPoint(transform.position), hitTransform: transform);
                
                DisableHitCheck(true);
            }
        }

        public void DisableHitCheck(bool disable)
        {
            _disableHitCheck = disable;
        }
    }
}