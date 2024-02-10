using Infrastructure;
using Infrastructure.States.Interfaces;
using UnityEngine;

namespace Gameplay.Player.Animation
{
    public class LumberjackAnimationRunState : IState
    {
        private readonly Animator _animator;
        
        private readonly int _runId = Animator.StringToHash("IsRunning");

        public LumberjackAnimationRunState(Animator animator)
        {
            _animator = animator;
        }

        public void Exit()
        {
            _animator.SetBool(_runId, false);
        }

        public void Enter()
        {
            _animator.SetBool(_runId, true);       
        }
    }
}