using Infrastructure.Services.Input;
using UnityEngine;
using Zenject;

namespace Infrastructure.States
{
    public class MainLocation : MonoBehaviour
    {
        [SerializeField] private Transform _playerInitialPoint;
        private IInputService _inputService;

        public Vector3 PlayerInitialPosition => _playerInitialPoint.position;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
    }
}