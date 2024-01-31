using UnityEngine;

namespace Infrastructure.StaticData.PlayerData
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "StaticData/Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _axeChopRadius;
        [SerializeField] private LayerMask _hitLayer;

        public float MovementSpeed => _movementSpeed;
        public float AxeChopRadius => _axeChopRadius;
        public LayerMask HitLayer => _hitLayer;
    }
}