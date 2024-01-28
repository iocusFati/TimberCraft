using Infrastructure.Services.Input;
using Infrastructure.StaticData.PlayerData;
using UnityEngine;
using Zenject;

namespace Infrastructure.States
{
    public class PlayerMovement
    {
        private readonly CharacterController _characterController;
        private readonly IInputService _inputService;
        private readonly float _movementSpeed;

        private Transform _cameraTransform;
        private Transform _playerTransform;

        public PlayerMovement(CharacterController characterController,
            IInputService inputService,
            PlayerConfig playerConfig, 
            Transform playerTransform)
        {
            _characterController = characterController;
            _movementSpeed = playerConfig.MovementSpeed;
            _inputService = inputService;
            _playerTransform = playerTransform;
        }

        public void SetCamera(Transform cameraTransform)
        {
            _cameraTransform = cameraTransform;
        }

        public void MoveCharacter(Vector3 movementVector) => 
            _characterController.Move(movementVector * (_movementSpeed * Time.deltaTime));

        public Vector3 GetMovementVector()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = _cameraTransform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();

                _playerTransform.forward = movementVector;
            }

            movementVector += Physics.gravity;
            return movementVector;
        }
    }
}