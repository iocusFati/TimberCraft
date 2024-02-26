using UnityEngine;

namespace Gameplay.Buildings
{
    public class GameCameraStateController : MonoBehaviour, IGameCameraController
    {
        [SerializeField] private Animator _cameraStateMachineAnimator;
        
        private readonly int _topViewId = Animator.StringToHash("TopView");
        private readonly int _playerCamera = Animator.StringToHash("FollowPlayer");

        public void SwitchToTopViewCamera() => 
            _cameraStateMachineAnimator.SetTrigger(_topViewId);

        public void SwitchToPlayerCamera() => 
            _cameraStateMachineAnimator.SetTrigger(_playerCamera);
    }
}