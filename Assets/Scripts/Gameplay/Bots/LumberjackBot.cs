using Gameplay.Bots.StateMachine;
using Gameplay.Bots.StateMachine.States;
using Gameplay.Locations;
using Gameplay.Lumberjack;
using Gameplay.Resource;
using Infrastructure;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.ResourcesData;
using Pathfinding;
using UnityEngine;
using Zenject;

namespace Gameplay.Bots
{
    [RequireComponent(typeof(AIPath))]
    public class LumberjackBot : LumberjackBase
    {
        [SerializeField] private Transform _lootStackBottom;

        private AIPath _aiPath;
        private MinionHut _hut;

        private LumberjackBotStateMachine _botStateMachine;
        private LumberjackStorageResourceShare _lumberjackResourceShare;
        private ICoroutineRunner _coroutineRunner;
        private ResourcesConfig _resourcesConfig;

        public ResourceType TargetResourceType { get; private set; }

        [Inject]
        public void Construct(ICoroutineRunner coroutineRunner, IStaticDataService staticData)
        {
            _coroutineRunner = coroutineRunner;
            _resourcesConfig = staticData.ResourcesConfig;

            _lumberjackStorage = new LumberjackStorage(staticData.LumberjackBotConfig, _lootStackBottom);
            _lumberjackStorage.Initialize();
            
            _lumberjackResourceShare = new LumberjackStorageResourceShare(
                _lumberjackStorage, _coroutineRunner, staticData.ResourcesConfig);
        }

        private void Awake()
        {
            _aiPath = GetComponent<AIPath>();
        }

        public void Initialize(ResourceSourcesHolder island, MinionHut hut)
        {
            _botStateMachine = new LumberjackBotStateMachine();
            TargetResourceType = hut.BotResourceType;
            _hut = hut;

            RegisterBotStates(island, _aiPath);
            
            _botStateMachine.Enter<GoToResourceSourceLumberjackBotState>();
        }

        protected override void CollectDropout(DropoutResource dropout)
        {
            base.CollectDropout(dropout);

            if (_lumberjackStorage.IsFull)
            {
                dropout.GetCollectedAndReleasedTo(_resourceCollector);
            }
            else
            {
                dropout.GetCollectedAndKeptTo(_lumberjackStorage.GetFreeCell());
                _lumberjackStorage.OccupyFreePosition(dropout);
            }
        }

        protected override bool CanCollectDropout(DropoutResource dropout) => 
            base.CanCollectDropout(dropout) && !_lumberjackStorage.IsFull;

        private void RegisterBotStates(ResourceSourcesHolder island, AIPath aiPath)
        {
            _botStateMachine.RegisterState(
                new GoToResourceSourceLumberjackBotState(island, this, aiPath, _botStateMachine, _lumberjackAnimator,
                    _lumberjackStorage, _coroutineRunner, _resourcesConfig));
            _botStateMachine.RegisterState(
                new IsOnTheWayToResourceSourceLumberjackBotState(_botStateMachine, _coroutineRunner, aiPath));
            _botStateMachine.RegisterState(
                new MineResourceLumberjackBotState(_botStateMachine, _lumberjackAnimator, _lumberjackStorage));
            _botStateMachine.RegisterState(
                new BringResourcesToHutLumberjackBotState(aiPath, _hut, _lumberjackAnimator, _lumberjackResourceShare,
                    _botStateMachine));
        }
    }
}