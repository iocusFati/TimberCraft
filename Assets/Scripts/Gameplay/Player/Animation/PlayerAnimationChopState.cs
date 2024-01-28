using UnityEngine;

namespace Infrastructure.States
{
    public class PlayerAnimationChopState : IState
    {
        private readonly Animator _animator;
        
        private readonly int _chopId = Animator.StringToHash("IsChopping");

        public PlayerAnimationChopState(Animator animator)
        {
            _animator = animator;
        }

        public void Exit()
        {
            _animator.SetBool(_chopId, false);
        }

        public void Enter()
        {
            _animator.SetBool(_chopId, true);       
        }
    }
}