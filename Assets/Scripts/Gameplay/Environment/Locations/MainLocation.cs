using System.Collections.Generic;
using Infrastructure.Services.Input;
using UnityEngine;
using Zenject;

namespace Gameplay.Environment.Locations
{
    public class MainLocation : MonoBehaviour
    {
        [SerializeField] private Transform _playerInitialPoint;
        [SerializeField] private List<Island> _islands;

        public Vector3 PlayerInitialPosition => _playerInitialPoint.position;

        [Inject]
        public void Construct(IInputService inputService)
        {
        }

        public void Initialize()
        {
            foreach (var island in _islands)
            {
                island.Initialize();
            }
        }
    }
}