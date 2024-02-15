using System.Collections.Generic;
using Gameplay.Player;
using Gameplay.Player.Animation;
using Gameplay.Resource;
using Infrastructure;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Input;
using Infrastructure.Services.StaticDataService;
using UnityEngine;
using Zenject;

namespace Gameplay.Lumberjack
{
    public class LumberjackBase : MonoBehaviour
    {
        protected const string PlayerCameraTag = "PlayerCamera";
        private const string ResourceTag = "Resource";
        private const string ResourceDropoutTag = "ResourceDropout";

        [SerializeField] private Animator _animator;
        [SerializeField] private TriggerInteraction _triggerInteraction;
        [SerializeField] private TriggerInteraction _lootTrigger;
        [SerializeField] protected LumberjackAxeEnabler _lumberjackAxeEnabler;
        [SerializeField] private LumberjackAxe _lumberjackAxe;
        [SerializeField] protected Transform _resourceCollector;

        protected LumberjackAnimator _lumberjackAnimator;

        protected ICoroutineRunner _coroutineRunner;
        protected ICacheService _cacheService;
        protected IGameResourceStorage _gameResourceStorage;

        private readonly HashSet<GameObject> _enteredResources = new();

        [Inject]
        public virtual void Construct(IInputService inputService,
            IStaticDataService staticData,
            ICacheService cacheService,
            ICoroutineRunner coroutineRunner, 
            IGameResourceStorage gameResourceStorage)
        {
            _cacheService = cacheService;
            _coroutineRunner = coroutineRunner;
            _gameResourceStorage = gameResourceStorage;
            
            _lumberjackAnimator = new LumberjackAnimator(_animator);
        }

        protected virtual void Start()
        {
            _lumberjackAxeEnabler.Construct(_lumberjackAxe);

            _triggerInteraction.OnTriggerEntered += OnTriggerInteractionEntered;
            _triggerInteraction.OnTriggerStayed += OnTriggerInteractionStayed;
            _triggerInteraction.OnTriggerExited += OnTriggerInteractionExited;

            _lootTrigger.OnTriggerEntered += OnLootTriggerEntered;
            _lootTrigger.OnTriggerExited += OnLootTriggerExited; 

            _lumberjackAxe.OnDestroyedResourceSource += StopChopping;
        }

        protected virtual void OnTriggerInteractionEntered(Collider other)
        {
            if (other.CompareTag(ResourceTag))
            {
                _enteredResources.Add(other.gameObject);
                _lumberjackAnimator.Chop();
            }
        }

        protected virtual void OnTriggerInteractionStayed(Collider other)
        {
            
        }

        protected virtual void OnTriggerInteractionExited(Collider other)
        {
            if (other.CompareTag(ResourceTag))
            {
                _enteredResources.Remove(other.gameObject);
                
                if (_enteredResources.Count == 0) 
                    StopChopping();
            }
        }

        private void OnLootTriggerEntered(Collider other)
        {
            if (other.CompareTag(ResourceDropoutTag)) 
                TryCollectDropout(CachedDropout(other));
        }

        private void OnLootTriggerExited(Collider other)
        {
            
        }

        protected virtual void CollectDropout(DropoutResource dropout)
        {
        }

        private void TryCollectDropout(DropoutResource dropout)
        {
            if (CanCollectDropout(dropout))
                CollectDropout(dropout);
        }

        protected virtual bool CanCollectDropout(DropoutResource dropout) => 
            !dropout.IsCollected;

        private DropoutResource CachedDropout(Component resourceSourceCollider) => 
            _cacheService.ResourceDropout.Get(resourceSourceCollider.gameObject);

        private void StopChopping()
        {
            _lumberjackAnimator.StopChopping();
            _lumberjackAxeEnabler.DisableAxeCollider();
        }
    }
}