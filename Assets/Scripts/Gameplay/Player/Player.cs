using Cinemachine;
using Gameplay.Bots.StateMachine.States;
using Gameplay.Lumberjack;
using Gameplay.Player.Animation;
using Gameplay.Resource;
using Infrastructure;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Input;
using Infrastructure.Services.Pool;
using Infrastructure.Services.StaticDataService;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Player
{
    public class Player : LumberjackBase
    {
        [SerializeField] private Transform _playerCameraLookAt;
        [SerializeField] private Transform _resourcesSpawnTransform;
        [SerializeField] private CharacterController _characterController;

        private CinemachineVirtualCamera _playerCamera;

        private PlayerMovement _playerMovement;
        private bool _isMoving;
        private PlayerResourceShare _resourceShare;

        [Inject]
        public void Construct(IInputService inputService,
            IStaticDataService staticData,
            ICacheService cacheService,
            ICoroutineRunner coroutineRunner,
            IGameResourceStorage gameResourceStorage, 
            IPoolService poolService)
        {
            base.Construct(inputService, staticData, cacheService, coroutineRunner, gameResourceStorage);
            
            _playerMovement = new PlayerMovement(_characterController, inputService, staticData.PlayerConfig, transform);
            _resourceShare = new PlayerResourceShare(gameResourceStorage, _resourcesSpawnTransform,
                staticData.PlayerConfig, poolService, coroutineRunner);
        }

        protected override void Start()
        {
            base.Start();
            
            InitializePlayerCamera();
            
            _playerMovement.SetCamera(_playerCamera.transform);
        }
        
        private void Update()
        {
            Vector3 movementVector = _playerMovement.GetMovementVector();

            Move(movementVector);
        }

        protected override void OnTriggerStayed(Collider other)
        {
            base.OnTriggerStayed(other);
            
            if (other.CompareTag(Tags.Building))
            {
                Building building = _cacheService.Buildings.Get(other.gameObject);

                _resourceShare.ShareForConstructionWith(building);
            }
        }

        protected override void CollectDropout(DropoutResource dropout)
        {
            base.CollectDropout(dropout);
            
            dropout.GetCollectedAndReleasedTo(_resourceCollector);
            _gameResourceStorage.TakeResource(dropout.Type, dropout.ResourceValue);
        }

        private void Move(Vector3 movementVector)
        {
            Vector2 movementVector2D = new Vector2(movementVector.x, movementVector.z);
            _playerMovement.MoveCharacter(movementVector);

            if (movementVector2D != Vector2.zero)
                IsMoving();
            else
                IsStatic();
            return;

            void IsMoving()
            {
                _lumberjackAnimator.Enter<LumberjackAnimationRunState>();

                _isMoving = true;
            }
            void IsStatic()
            {
                if (_isMoving)
                {
                    _lumberjackAnimator.Enter<LumberjackAnimationIdleState>();

                    _isMoving = false;
                }
            }
        }
        
        private void InitializePlayerCamera()
        {
            _playerCamera = GameObject
                .FindWithTag(PlayerCameraTag)
                .GetComponent<CinemachineVirtualCamera>();

            _playerCamera.m_Follow = transform;
            _playerCamera.m_LookAt = _playerCameraLookAt;
        }
    }
}