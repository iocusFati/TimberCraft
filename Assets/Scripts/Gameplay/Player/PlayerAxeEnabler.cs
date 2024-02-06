using UnityEngine;

namespace Infrastructure.States
{
    public class PlayerAxeEnabler : MonoBehaviour
    {
        private PlayerAxe _axe;

        public void Construct(PlayerAxe playerAxe)
        {
            _axe = playerAxe;
        }

        public void DisableAxeCollider() => 
            _axe.DisableHitCheck(true);

        public void EnableAxeCollider() => 
            _axe.DisableHitCheck(false);
    }
}