using Cinemachine;
using ECM.Components;
using Gameplay.Buildings;
using Gameplay.Lumberjack;
using Gameplay.Player.Animation;
using Gameplay.Resource;
using Gameplay.Resource.ResourceStorage;
using Infrastructure;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Input;
using Infrastructure.Services.Pool;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.CameraData;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Player
{
    public class Player : LumberjackBase
    {
        [SerializeField] private Transform _resourcesSpawnTransform;
        [SerializeField] private CharacterMovement _characterMovement;

        [Header("Camera")]
        [SerializeField] private Transform _playerCameraLookAt;

        private bool _isMoving;
        
        private PlayerMovement _playerMovement;
        private CameraSway _cameraSway;

        public CinemachineVirtualCamera PlayerCamera { get; private set; }
        public PlayerResourceShare ResourceShare { get; private set; }

        [Inject]
        public void Construct(IInputService inputService,
            IStaticDataService staticData,
            ICacheService cacheService,
            ICoroutineRunner coroutineRunner,
            IGameResourceStorage gameResourceStorage, 
            IPoolService poolService)
        {
            base.Construct(inputService, staticData, cacheService, coroutineRunner, gameResourceStorage);
            
            _playerMovement = new PlayerMovement(_characterMovement, inputService, staticData.PlayerConfig, transform);
            ResourceShare = new PlayerResourceShare(gameResourceStorage, _resourcesSpawnTransform,
                staticData.PlayerConfig, poolService, coroutineRunner);
        }

        protected override void Start()
        {
            base.Start();
            
            InitializePlayerCamera();
            
            _playerMovement.SetCamera(PlayerCamera.transform);
        }
        
        private void FixedUpdate()
        {
            Vector3 movementVector = _playerMovement.GetMovementVector();

            Move(movementVector);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.BuildingTrigger))
            {
                Building building = _cacheService.Buildings.Get(other.gameObject);

                if (building.IsBuilt) 
                    building.InteractWithPlayer();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.BuildingTrigger))
            {
                Building building = _cacheService.Buildings.Get(other.gameObject);

                if (building.IsBuilt) 
                    building.StopInteractingWithPlayer();
            }
        }

        protected override void OnTriggerInteractionEntered(Collider other)
        {
            base.OnTriggerInteractionEntered(other);
            
            if (other.CompareTag(Tags.ResourceSource)) 
                _resourceSourceCache.Get(other.gameObject).StartMining();
        }

        protected override void OnTriggerInteractionExited(Collider other)
        {
            base.OnTriggerInteractionExited(other);
            
            if (other.CompareTag(Tags.ResourceSource)) 
                _resourceSourceCache.Get(other.gameObject).StopMining();
        }

        protected override async void OnTriggerInteractionStayed(Collider other)
        {
            base.OnTriggerInteractionStayed(other);
            
            if (other.CompareTag(Tags.Building))
            {
                Building building = _cacheService.Buildings.Get(other.gameObject);

                if (!building.IsBuilt) 
                    await ResourceShare.ShareResourcesForConstructionWith(building);
            }
        }

        protected override void CollectDropout(DropoutResource dropout)
        {
            base.CollectDropout(dropout);
            
            dropout.GetCollectedAndReleasedTo(_resourceCollector, OnDropoutCollected);
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
            PlayerCamera = GameObject
                .FindWithTag(PlayerCameraTag)
                .GetComponent<CinemachineVirtualCamera>();

            PlayerCamera.m_Follow = transform;
            PlayerCamera.m_LookAt = _playerCameraLookAt;
        }
    }
}