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
        private readonly LumberjackStorageResourceShare _lumberjackStorageResourceShare;
        private readonly IStateMachine _botStateMachine;

        public BringResourcesToHutLumberjackBotState(AIPath aiPath,
            MinionHut hut,
            LumberjackAnimator lumberjackAnimator,
            LumberjackStorageResourceShare lumberjackStorageResourceShare, 
            IStateMachine botStateMachine)
        {
            _aiPath = aiPath;
            _hut = hut;
            _lumberjackAnimator = lumberjackAnimator;
            _lumberjackStorageResourceShare = lumberjackStorageResourceShare;
            _botStateMachine = botStateMachine;
        }

        public void Enter()
        {
            _aiPath.destination = _hut.TriggerZoneTransform.position;
            
            _lumberjackAnimator.Run();

            _aiPath.OnTargetWasReached += OnHutReached;
        }

        private void OnHutReached()
        {
            _lumberjackAnimator.Idle();
            _lumberjackStorageResourceShare.ShareAllResources(_hut);
            
            _botStateMachine.Enter<GoToResourceSourceLumberjackBotState>();
            
            _aiPath.OnTargetWasReached -= OnHutReached;
        }

        public void Exit()
        {
        }
    }
}