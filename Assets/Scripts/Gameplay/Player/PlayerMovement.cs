using ECM.Components;
using Infrastructure.Services.Input;
using Infrastructure.StaticData.LumberjackData;
using UnityEngine;
using Utils;

namespace Gameplay.Player
{
    public class PlayerMovement
    {
        private readonly CharacterMovement _characterMovement;
        private readonly IInputService _inputService;
        private readonly float _movementSpeed;

        private Transform _cameraTransform;
        private Transform _playerTransform;

        public PlayerMovement(CharacterMovement characterMovement,
            IInputService inputService,
            PlayerConfig playerConfig, 
            Transform playerTransform)
        {
            _characterMovement = characterMovement;
            _movementSpeed = playerConfig.MovementSpeed;
            _inputService = inputService;
            _playerTransform = playerTransform;
        }

        public void SetCamera(Transform cameraTransform)
        {
            _cameraTransform = cameraTransform;
        }

        public void MoveCharacter(Vector3 movementVector)
        {
            _characterMovement.Move(movementVector * _movementSpeed, _movementSpeed);
        }

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