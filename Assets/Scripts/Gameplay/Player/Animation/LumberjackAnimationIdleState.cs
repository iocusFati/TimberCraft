using Infrastructure.States.Interfaces;
using UnityEngine;

namespace Gameplay.Player.Animation
{
    public class LumberjackAnimationIdleState : IState
    {
        private readonly Animator _animator;
        
        private readonly int _idleId = Animator.StringToHash("Idle");

        public LumberjackAnimationIdleState(Animator animator)
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