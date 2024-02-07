using Cinemachine;
using Infrastructure.Services.Input;
using Infrastructure.Services.StaticDataService;
using UnityEngine;
using Zenject;

namespace Infrastructure.States
{
    public class LumberjackBase : MonoBehaviour
    {
        protected const string PlayerCameraTag = "PlayerCamera";
        private const string ResourceTag = "Resource";
        private const string ResourceDropoutTag = "ResourceDropout";

        [SerializeField] private Animator _animator;
        [SerializeField] private TriggerInteraction _triggerInteraction;
        [SerializeField] private TriggerInteraction _lootTrigger;
        [SerializeField] protected PlayerAxeEnabler _playerAxeEnabler;
        [SerializeField] private PlayerAxe _playerAxe;
        [SerializeField] private Transform _resourceCollector;

        protected LumberjackAnimator _lumberjackAnimator;
        private ICacheService _cacheService;
        private LumberjackStorage _lumberjackStorage;

        [Inject]
        public virtual void Construct(
            IInputService inputService, 
            IStaticDataService staticData,
            ICacheService cacheService)
        {
            _cacheService = cacheService;
            
            _lumberjackAnimator = new LumberjackAnimator(_animator);
        }

        protected virtual void Start()
        {
            _playerAxeEnabler.Construct(_playerAxe);
            
            _triggerInteraction.OnTriggerEntered += OnTriggerEntered;
            _triggerInteraction.OnTriggerExited += OnTriggerExited;

            _lootTrigger.OnTriggerEntered += OnLootTriggerEntered;
            _lootTrigger.OnTriggerExited += OnLootTriggerExited; 

            _playerAxe.OnDestroyedResourceSource += StopChopping;
        }
        
        private void OnTriggerEntered(Collider other)
        {
            if (other.CompareTag(ResourceTag))
            {
                _lumberjackAnimator.Chop();
            }
        }

        private void OnTriggerExited(Collider other)
        {
            if (other.CompareTag(ResourceTag)) 
                StopChopping();
        }

        private void OnLootTriggerEntered(Collider other)
        {
            if (other.CompareTag(ResourceDropoutTag))
            {
                DropoutResource cachedDropout = _cacheService.ResourceDropout.Get(other.gameObject);

                cachedDropout.GetCollectedTo(_resourceCollector.position);
            }
        }

        private void OnLootTriggerExited(Collider other)
        {
            
        }

        private void StopChopping()
        {
            _lumberjackAnimator.StopChopping();
            _playerAxeEnabler.DisableAxeCollider();
        }
    }
}