using System.Collections;
using Gameplay.Player;
using Infrastructure;
using Infrastructure.States.Interfaces;
using Pathfinding;

namespace Gameplay.Bots.StateMachine.States
{
    public class IsOnTheWayToResourceSourceLumberjackBotState : IPayloadedState<ResourceSource>
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IStateMachine _botStateMachine;
        private readonly AIPath _aiPath;

        private ResourceSource _targetSource;
        private bool _stateIsActive;

        public IsOnTheWayToResourceSourceLumberjackBotState(
            IStateMachine botStateMachine, 
            ICoroutineRunner coroutineRunner,
            AIPath aiPath)
        {
            _botStateMachine = botStateMachine;
            _coroutineRunner = coroutineRunner;
            _aiPath = aiPath;
        }

        public void Enter(ResourceSource targetSource)
        {
            _stateIsActive = true;
            _targetSource = targetSource;
            
            _coroutineRunner.StartCoroutine(ChangeTargetResourceSourceIfCurrentIsDestroyed(targetSource));
            
            _aiPath.OnTargetWasReached += OnOnTargetWasReached;
        }

        private void OnOnTargetWasReached()
        {
            _botStateMachine.Enter<MineResourceLumberjackBotState, ResourceSource>(_targetSource);
        }

        public void Exit()
        {
            _stateIsActive = false;
            
            _aiPath.OnTargetWasReached -= OnOnTargetWasReached;
        }

        private IEnumerator ChangeTargetResourceSourceIfCurrentIsDestroyed(ResourceSource targetSource)
        {
            while (_stateIsActive)
            {
                if (ResourceSourceIsMined())
                {
                    ChangeResourceSource();

                    break;
                }

                yield return null;
            }

            bool ResourceSourceIsMined() => 
                targetSource.CurrentState == ResourceSourceState.Mined;
        }

        private void ChangeResourceSource() => 
            _botStateMachine.Enter<GoToResourceSourceLumberjackBotState>();
    }
}