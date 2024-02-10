using Gameplay.Lumberjack;
using Gameplay.Player;
using Gameplay.Player.Animation;
using Infrastructure.States.Interfaces;

namespace Gameplay.Bots.StateMachine.States
{
    public class MineResourceLumberjackBotState : IPayloadedState<ResourceSource>
    {
        private readonly IStateMachine _botStateMachine;
        private readonly LumberjackAnimator _lumberjackAnimator;
        private readonly LumberjackStorage _lumberjackStorage;
        
        private ResourceSource _resourceSource;

        public MineResourceLumberjackBotState(IStateMachine botStateMachine, LumberjackAnimator lumberjackAnimator, LumberjackStorage lumberjackStorage)
        {
            _botStateMachine = botStateMachine;
            _lumberjackAnimator = lumberjackAnimator;
            _lumberjackStorage = lumberjackStorage;
        }

        public void Enter(ResourceSource resourceSource)
        {
            _resourceSource = resourceSource;
            _resourceSource.OnResourceMined += OnResourceMined;

            _lumberjackAnimator.Idle();
        }

        private void OnResourceMined()
        {
            if (!_lumberjackStorage.IsFull)
                _botStateMachine.Enter<GoToResourceSourceLumberjackBotState>();
            else
                _botStateMachine.Enter<BringResourcesToHutLumberjackBotState>();
            
            _resourceSource.OnResourceMined -= OnResourceMined;
        }

        public void Exit()
        {
            
        }
    }
}