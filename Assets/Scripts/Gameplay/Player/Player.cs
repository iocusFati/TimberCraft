using Cinemachine;
using Gameplay.Player.Animation;
using Infrastructure.Services.Input;
using Infrastructure.Services.StaticDataService;
using UnityEngine;
using Zenject;

namespace Infrastructure.States
{
    public class Player : MonoBehaviour
    {
        private const string PlayerCameraTag = "PlayerCamera";
        private const string ResourceTag = "Resource";

        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _playerCameraLookAt;
        [SerializeField] private TriggerInteraction _triggerInteraction;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PlayerAxeEnabler _playerAxeEnabler;

        private CinemachineVirtualCamera _playerCamera;
        private PlayerMovement _playerMovement;
        private PlayerAnimator _playerAnimator;
        // private PlayerAxe _playerAxe;

        private bool _isMoving;

        [Inject]
        public void Construct(IInputService inputService, IStaticDataService staticData)
        {
            _playerMovement = new PlayerMovement(_characterController, inputService, staticData.PlayerConfig, transform);
            _playerAnimator = new PlayerAnimator(_animator);
            // _playerAxe = new PlayerAxe(_axeHead, staticData.PlayerConfig);
            
            // _playerAxeEnabler.Construct(_playerAxe);
        }

        private void Start()
        {
            InitializePlayerCamera();

            _playerMovement.SetCamera(_playerCamera.transform);
            
            _triggerInteraction.OnTriggerEntered += OnTriggerEntered;
            _triggerInteraction.OnTriggerExited += OnTriggerExited;
        }

        private void Update()
        {
            Vector3 movementVector = _playerMovement.GetMovementVector();

            Move(movementVector);

            // if (_playerAxe.HasHitTheTree(out var hitResourceSources))
            // {
            //     foreach (var resourceSource in hitResourceSources)
            //     {
            //         _playerAxe.DamageResourceSource(resourceSource);
            //     }
            // }
        }

        private void OnTriggerEntered(Collider other)
        {
            if (other.CompareTag(ResourceTag))
            {
                _playerAnimator.Chop();
                _playerAxeEnabler.EnableAxeCollider();
            }
        }

        private void OnTriggerExited(Collider other)
        {
            if (other.CompareTag(ResourceTag))
            {
                _playerAnimator.StopChopping();
                _playerAxeEnabler.DisableAxeCollider();
            }
        }
        
        private void Move(Vector3 movementVector)
        {
            Vector2 movementVector2D = new Vector2(movementVector.x, movementVector.z);
            _playerMovement.MoveCharacter(movementVector);

            if (movementVector2D != Vector2.zero)
                IsMoving();
            else
                IsStatic();

            void IsMoving()
            {
                _playerAnimator.Enter<PlayerAnimationRunState>();

                _isMoving = true;
            }
            void IsStatic()
            {
                if (_isMoving)
                {
                    _playerAnimator.Enter<PlayerAnimationIdleState>();

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