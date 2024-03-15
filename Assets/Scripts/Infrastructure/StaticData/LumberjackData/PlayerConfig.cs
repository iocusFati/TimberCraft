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
        [SerializeField] private float _resourceDeliveryCooldown;
        
        [Header("Axe")]
        [SerializeField] private int _treeMaxDamagesPerSwing;
        [SerializeField] private int _stoneMaxDamagesPerSwing;

        public float MovementSpeed => _movementSpeed;
        public int ResourcesInOneDropout => _resourcesInOneDropout;
        public float ResourceDeliveryCooldown => _resourceDeliveryCooldown;

        public int TreeMaxDamagesPerSwing => _treeMaxDamagesPerSwing;

        public int StoneMaxDamagesPerSwing => _stoneMaxDamagesPerSwing;
    }
}