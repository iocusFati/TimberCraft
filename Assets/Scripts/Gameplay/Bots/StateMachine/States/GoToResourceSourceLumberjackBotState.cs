using System.Collections;
using Gameplay.Environment.Locations;
using Gameplay.Lumberjack;
using Gameplay.Player.Animation;
using Gameplay.Resource;
using Infrastructure;
using Infrastructure.States.Interfaces;
using Infrastructure.StaticData.ResourcesData;
using Pathfinding;
using UnityEngine;

namespace Gameplay.Bots.StateMachine.States
{
    public class GoToResourceSourceLumberjackBotState : IState
    {
        private readonly ResourceSourcesHolder _resourceSourcesHolder;
        private readonly LumberjackBot _bot;
        private readonly AIPath _aiPath;
        private readonly IStateMachine _botStateMachine;
        private readonly LumberjackAnimator _lumberjackAnimator;
        private readonly LumberjackBotStorage _lumberjackBotStorage;
        private readonly ICoroutineRunner _coroutineRunner;
        
        private readonly float _tryToFindResourceAgainTime;
        
        private ResourceSource _targetResource;

        public GoToResourceSourceLumberjackBotState(ResourceSourcesHolder resourceSourcesHolder,
            LumberjackBot bot,
            AIPath aiPath,
            IStateMachine botStateMachine,
            LumberjackAnimator lumberjackAnimator,
            LumberjackBotStorage lumberjackBotStorage,
            ICoroutineRunner coroutineRunner, 
            ResourcesConfig resourceConfig)
        {
            _resourceSourcesHolder = resourceSourcesHolder;
            _bot = bot;
            _aiPath = aiPath;
            _botStateMachine = botStateMachine;
            _lumberjackAnimator = lumberjackAnimator;
            _lumberjackBotStorage = lumberjackBotStorage;
            _coroutineRunner = coroutineRunner;

            _tryToFindResourceAgainTime = resourceConfig.TryToFindResourceAgainTime;
            
            _lumberjackBotStorage.OnStorageFull += BringResourcesToHut;
        }

        public void Enter()
        {
            _targetResource = GetTargetResource();

            if (FailToFindResourceSource(_targetResource)) 
                return;

            _targetResource.StartMining();
            _aiPath.destination = _targetResource.transform.position;
            _lumberjackAnimator.Run();
            
            _botStateMachine.Enter<IsOnTheWayToResourceSourceLumberjackBotState, ResourceSource>(_targetResource);
        }

        public void Exit()
        {
            
        }

        private void BringResourcesToHut()
        {
            _targetResource.StopMining();
            _botStateMachine.Enter<BringResourcesToHutLumberjackBotState>();
        }

        private IEnumerator WaitAndRepeat()
        {
            _lumberjackAnimator.Idle();
            
            yield return new WaitForSeconds(_tryToFindResourceAgainTime);
            
            Enter();
        }

        private bool FailToFindResourceSource(ResourceSource resource)
        {
            if (resource is not null) 
                return false;
            
            if (_lumberjackBotStorage.ResourceDropouts.Count > 0)
                _botStateMachine.Enter<BringResourcesToHutLumberjackBotState>();
            else
                _coroutineRunner.StartCoroutine(WaitAndRepeat());
                
            return true;
        }

        private ResourceSource GetTargetResource() =>
            _resourceSourcesHolder
                .GetClosestSourceOfType(_bot.TargetResourceType, _bot.transform.position) 
            ?? _resourceSourcesHolder
                .GetClosestSourceOfType(_bot.TargetResourceType, _bot.transform.position, ResourceSourceState.BeingMined);
    }
}