using System;
using System.Collections.Generic;
using Gameplay.Player.Animation;
using UnityEngine;

namespace Infrastructure.States
{
    public class PlayerAnimatorStateMachine : StateMachine
    {
        public PlayerAnimatorStateMachine(Animator animator)
        {
            _states = new Dictionary<Type, IExitState>
            {
                [typeof(PlayerAnimationRunState)] = new PlayerAnimationRunState(animator),
                [typeof(PlayerAnimationIdleState)] = new PlayerAnimationIdleState(animator),
                [typeof(PlayerAnimationChopState)] = new PlayerAnimationChopState(animator)
            };
        }
    }
}