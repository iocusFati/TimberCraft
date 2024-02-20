using Gameplay.Buildings;
using Gameplay.Locations;
using Gameplay.Player.Animation;
using Infrastructure.States.Interfaces;
using Pathfinding;

namespace Gameplay.Bots.StateMachine.States
{
    public class BringResourcesToHutLumberjackBotState : IState
    {
        private readonly AIPath _aiPath;
        private readonly MinionHut _hut;
        private readonly LumberjackAnimator _lumberjackAnimator;
        private readonly LumberjackBotStorageResourceShare _lumberjackBotStorageResourceShare;
        private readonly IStateMachine _botStateMachine;

        public BringResourcesToHutLumberjackBotState(AIPath aiPath,
            MinionHut hut,
            LumberjackAnimator lumberjackAnimator,
            LumberjackBotStorageResourceShare lumberjackBotStorageResourceShare, 
            IStateMachine botStateMachine)
        {
            _aiPath = aiPath;
            _hut = hut;
            _lumberjackAnimator = lumberjackAnimator;
            _lumberjackBotStorageResourceShare = lumberjackBotStorageResourceShare;
            _botStateMachine = botStateMachine;
        }

        public void Enter()
        {
            _aiPath.destination = _hut.SpawnBotsTransform.position;
                    
            _lumberjackAnimator.Run();

            _aiPath.OnTargetWasReached += OnHutReached;
        }

        private void OnHutReached()
        {
            _lumberjackAnimator.Idle();
            _lumberjackBotStorageResourceShare.ShareAllResources(_hut);
            
            _botStateMachine.Enter<GoToResourceSourceLumberjackBotState>();
            
            _aiPath.OnTargetWasReached -= OnHutReached;
        }

        public void Exit()
        {
        }
    }
}