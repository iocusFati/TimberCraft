using System;
using System.Collections.Generic;
using Gameplay.Player.Animation;
using UnityEngine;

namespace Infrastructure.States
{
    public class LumberjackAnimator : StateMachine
    {
        private readonly PlayerAnimationChop _chopAnimation;

        public LumberjackAnimator(Animator animator)
        {
            _chopAnimation = new PlayerAnimationChop(animator);
            
            _states = new Dictionary<Type, IExitState>
            {
                [typeof(PlayerAnimationRunState)] = new PlayerAnimationRunState(animator),
                [typeof(PlayerAnimationIdleState)] = new PlayerAnimationIdleState(animator),
            };
        }

        public void Chop()
        {
            _chopAnimation.StartChopping();
        }

        public void StopChopping()
        {
            _chopAnimation.StopChopping();
        }
    }
}