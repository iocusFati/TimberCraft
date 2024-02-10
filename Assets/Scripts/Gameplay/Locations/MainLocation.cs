using System.Collections.Generic;
using Infrastructure.Services.Input;
using UnityEngine;
using Zenject;

namespace Gameplay.Locations
{
    public class MainLocation : MonoBehaviour
    {
        [SerializeField] private Transform _playerInitialPoint;
        [SerializeField] private Island _mainIsland;
        
        private IInputService _inputService;

        public Vector3 PlayerInitialPosition => _playerInitialPoint.position;
        public Island MainIsland => _mainIsland;
        public List<Island> OpenedIslands { get; } = new();

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Initialize()
        {
            _mainIsland.Initialize();
        }
    }
}