using UnityEngine;

namespace Infrastructure.StaticData.LumberjackData
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "StaticData/Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float _movementSpeed;
        
        [Header("Resources")]
        [SerializeField] private int _resourcesInOneDropout;
        [SerializeField] private float _deliverResourceDuration;
        [SerializeField] private float _resourceDeliveryCooldown;

        public float MovementSpeed => _movementSpeed;
        public int ResourcesInOneDropout => _resourcesInOneDropout;
        public float DeliverResourceDuration => _deliverResourceDuration;
        public float ResourceDeliveryCooldown => _resourceDeliveryCooldown;
    }
}