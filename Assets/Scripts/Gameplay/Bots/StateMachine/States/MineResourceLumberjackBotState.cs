using Gameplay.Lumberjack;
using Gameplay.Player;
using Gameplay.Player.Animation;
using Gameplay.Resource;
using Infrastructure.States.Interfaces;

namespace Gameplay.Bots.StateMachine.States
{
    public class MineResourceLumberjackBotState : IPayloadedState<ResourceSource>
    {
        private readonly IStateMachine _botStateMachine;
        private readonly LumberjackAnimator _lumberjackAnimator;
        private readonly LumberjackBotStorage _lumberjackBotStorage;
        
        private ResourceSource _resourceSource;

        public MineResourceLumberjackBotState(IStateMachine botStateMachine, LumberjackAnimator lumberjackAnimator, LumberjackBotStorage lumberjackBotStorage)
        {
            _botStateMachine = botStateMachine;
            _lumberjackAnimator = lumberjackAnimator;
            _lumberjackBotStorage = lumberjackBotStorage;
        }

        public void Enter(ResourceSource resourceSource)
        {
            _resourceSource = resourceSource;
            _resourceSource.OnResourceMined += OnResourceMined;

            _lumberjackAnimator.Idle();
        }

        private void OnResourceMined(ResourceSource resourceSource)
        {
            if (!_lumberjackBotStorage.IsFull)
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