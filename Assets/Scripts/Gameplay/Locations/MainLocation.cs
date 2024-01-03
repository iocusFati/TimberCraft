using UnityEngine;

namespace Infrastructure.States
{
    public class MainLocation : MonoBehaviour
    {
        [SerializeField] private Transform _playerInitialPoint;
        
        public Vector3 PlayerInitialPosition => _playerInitialPoint.position;
    }
}