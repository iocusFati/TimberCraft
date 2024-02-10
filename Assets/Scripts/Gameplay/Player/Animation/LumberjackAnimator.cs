using System;
using System.Collections.Generic;
using Infrastructure.States;
using Infrastructure.States.Interfaces;
using UnityEngine;

namespace Gameplay.Player.Animation
{
    public class LumberjackAnimator : StateMachine
    {
        private readonly LumberjackAnimationChop _chopAnimation;

        public LumberjackAnimator(Animator animator)
        {
            _chopAnimation = new LumberjackAnimationChop(animator);
            
            _states = new Dictionary<Type, IExitState>
            {
                [typeof(LumberjackAnimationRunState)] = new LumberjackAnimationRunState(animator),
                [typeof(LumberjackAnimationIdleState)] = new LumberjackAnimationIdleState(animator),
            };
        }

        public void Chop() => 
            _chopAnimation.StartChopping();

        public void StopChopping() => 
            _chopAnimation.StopChopping();
        
        public void Run() => 
            Enter<LumberjackAnimationRunState>();

        public void Idle() => 
            Enter<LumberjackAnimationIdleState>();
    }
}