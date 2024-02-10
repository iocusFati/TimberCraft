using UnityEngine;

namespace Gameplay.Lumberjack
{
    public class LumberjackAxeEnabler : MonoBehaviour
    {
        private LumberjackAxe _axe;

        public void Construct(LumberjackAxe lumberjackAxe)
        {
            _axe = lumberjackAxe;
        }

        public void DisableAxeCollider() => 
            _axe.DisableHitCheck(true);

        public void EnableAxeCollider() => 
            _axe.DisableHitCheck(false);
    }
}