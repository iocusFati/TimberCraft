using UnityEngine;

namespace Infrastructure.StaticData.PlayerData
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "StaticData/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float _movementSpeed;

        public float MovementSpeed => _movementSpeed;
    }
}