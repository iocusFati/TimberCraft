using UnityEngine;

namespace Infrastructure.States
{
    public class PlayerAnimationChop
    {
        private readonly Animator _animator;

        private readonly int _chopId = Animator.StringToHash("IsChopping");
        
        private Coroutine _choppingCoroutine;

        public PlayerAnimationChop(Animator animator)
        {
            _animator = animator;
        }

        public void StartChopping()
        {
            Debug.Log("True");
            _animator.SetBool(_chopId, true);
        }

        public void StopChopping()
        {
            Debug.Log("False");
            _animator.SetBool(_chopId, false);
        }
    }
}