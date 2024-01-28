using UnityEngine;

namespace Infrastructure.States
{
    public class PlayerAnimationIdleState : IState
    {
        private readonly Animator _animator;
        
        private readonly int _idleId = Animator.StringToHash("Idle");

        public PlayerAnimationIdleState(Animator animator)
        {
            _animator = animator;
        }

        public void Exit()
        {
            _animator.SetBool(_idleId, false);
        }

        public void Enter()
        {
            _animator.SetBool(_idleId, true);       
        }
    }
}