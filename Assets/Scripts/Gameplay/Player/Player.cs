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

        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _playerCameraLookAt;
        [SerializeField] private CharacterController _characterController;

        private CinemachineVirtualCamera _playerCamera;
        private PlayerMovement _playerMovement;
        private PlayerAnimatorStateMachine _playerAnimator;
        
        private bool _isMoving;

        [Inject]
        public void Construct(IInputService inputService, IStaticDataService staticData)
        {
            _playerMovement = new PlayerMovement(_characterController, inputService, staticData.PlayerConfig, transform);
            _playerAnimator = new PlayerAnimatorStateMachine(_animator);
        }

        private void Start()
        {
            InitializePlayerCamera();

            _playerMovement.SetCamera(_playerCamera.transform);
        }

        private void Update()
        {
            Vector3 movementVector = _playerMovement.GetMovementVector();

            Move(movementVector);
        }

        private void Move(Vector3 movementVector)
        {
            Vector2 movementVector2D = new Vector2(movementVector.x, movementVector.z);
            _playerMovement.MoveCharacter(movementVector);

            if (movementVector2D != Vector2.zero)
            {
                _playerAnimator.Enter<PlayerAnimationRunState>();

                _isMoving = true;
            }
            else
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