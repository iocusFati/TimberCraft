using UnityEngine;

namespace Infrastructure.States
{
    public class PlayerAxeEnabler : MonoBehaviour
    {
        [SerializeField] private PlayerAxe _axe;
        
        public void DisableAxeCollider() => 
            _axe.DisableHitCheck(true);
        
        public void EnableAxeCollider() => 
            _axe.DisableHitCheck(false);
    }
}