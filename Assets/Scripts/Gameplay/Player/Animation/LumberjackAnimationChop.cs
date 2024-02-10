using UnityEngine;

namespace Gameplay.Player.Animation
{
    public class LumberjackAnimationChop
    {
        private readonly Animator _animator;

        private readonly int _chopId = Animator.StringToHash("IsChopping");
        
        private Coroutine _choppingCoroutine;

        public LumberjackAnimationChop(Animator animator)
        {
            _animator = animator;
        }

        public void StartChopping()
        {
            _animator.SetBool(_chopId, true);
        }

        public void StopChopping()
        {
            _animator.SetBool(_chopId, false);
        }
    }
}