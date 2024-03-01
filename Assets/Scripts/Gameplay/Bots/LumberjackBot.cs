using Gameplay.Bots.StateMachine;
using Gameplay.Bots.StateMachine.States;
using Gameplay.Buildings;
using Gameplay.Locations;
using Gameplay.Lumberjack;
using Gameplay.Resource;
using Gameplay.Resource.ResourceStorage;
using Gameplay.Resource.StoneFolder;
using Infrastructure;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Input;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.ResourcesData;
using Pathfinding;
using UnityEngine;
using Utils;
using Zenject;
using Tree = Gameplay.Resource.Tree;

namespace Gameplay.Bots
{
    [RequireComponent(typeof(AIPath))]
    public class LumberjackBot : LumberjackBase
    {
        [SerializeField] private Transform _lootStackBottom;

        private AIPath _aiPath;
        private MinionHut _hut;

        private LumberjackBotStateMachine _botStateMachine;
        private LumberjackBotStorageResourceShare _lumberjackBotResourceShare;
        
        private ResourcesConfig _resourcesConfig;
        
        public LumberjackBotStorage BotStorage { get; private set; }
        public ResourceType TargetResourceType { get; private set; }
        
        [Inject]
        public override void Construct(IInputService inputService,
            IStaticDataService staticData,
            ICacheService cacheService,
            ICoroutineRunner coroutineRunner, 
            IGameResourceStorage gameResourceStorage)
        {
            base.Construct(inputService, staticData, cacheService, coroutineRunner, gameResourceStorage);
            
            _resourcesConfig = staticData.ResourcesConfig;

            BotStorage = new LumberjackBotStorage(staticData.LumberjackBotConfig, _lootStackBottom);
            BotStorage.Initialize();
            
            _lumberjackBotResourceShare = new LumberjackBotStorageResourceShare(
                BotStorage, _coroutineRunner, staticData.ResourcesConfig, gameResourceStorage);
        }

        private void Awake()
        {
            _aiPath = GetComponent<AIPath>();

            _lumberjackAxe.SetTargetResourceType(TargetResourceType);
        }

        public void Initialize(ResourceSourcesHolder island, MinionHut hut)
        {
            _botStateMachine = new LumberjackBotStateMachine();
            TargetResourceType = hut.ConstructionResourceType;
            _hut = hut;

            RegisterBotStates(island, _aiPath);
            
            _botStateMachine.Enter<GoToResourceSourceLumberjackBotState>();
        }

        protected override void OnTriggerInteractionEntered(Collider other)
        {
            if (other.CompareTag(Tags.ResourceSource))
            {
                ResourceSource resourceSource = _resourceSourceCache.Get(other.gameObject);

                if (!resourceSource.CanBeMinedByBotWithType(TargetResourceType))
                    return;
            }
            
            base.OnTriggerInteractionEntered(other);
        }
        
        protected override void CollectDropout(DropoutResource dropout)
        {
            base.CollectDropout(dropout);

            if (BotStorage.IsFull)
            {
                dropout.GetCollectedAndReleasedTo(_resourceCollector, OnDropoutCollected);
            }
            else
            {
                dropout.GetCollectedAndKeptTo(BotStorage.GetFreeCell());
                BotStorage.OccupyFreePosition(dropout);
            }
        }

        protected override bool CanCollectDropout(DropoutResource dropout) => 
            base.CanCollectDropout(dropout) && !BotStorage.IsFull && dropout.Type == TargetResourceType;

        private void RegisterBotStates(ResourceSourcesHolder island, AIPath aiPath)
        {
            _botStateMachine.RegisterState(
                new GoToResourceSourceLumberjackBotState(island, this, aiPath, _botStateMachine, _lumberjackAnimator,
                    BotStorage, _coroutineRunner, _resourcesConfig));
            _botStateMachine.RegisterState(
                new IsOnTheWayToResourceSourceLumberjackBotState(_botStateMachine, _coroutineRunner, aiPath));
            _botStateMachine.RegisterState(
                new MineResourceLumberjackBotState(_botStateMachine, _lumberjackAnimator, BotStorage));
            _botStateMachine.RegisterState(
                new BringResourcesToHutLumberjackBotState(aiPath, _hut, _lumberjackAnimator, _lumberjackBotResourceShare,
                    _botStateMachine));
        }
    }
}