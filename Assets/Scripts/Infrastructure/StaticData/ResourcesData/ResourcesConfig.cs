using System;
using Gameplay.Resource;
using UnityEngine;

namespace Infrastructure.StaticData.ResourcesData
{
    [CreateAssetMenu(fileName = "ResourcesConfig", menuName = "StaticData/Configs/ResourcesConfig")]
    public class ResourcesConfig : ScriptableObject
    {
        [SerializeField] private float _fadeDuration;
        [SerializeField] private float _restoreTreeAfter;
        
        [Header("Dropout")]
        [SerializeField] private float _dropoutCollectDuration;

        [Header("Resource sharing")]
        [SerializeField] private float _deliverResourceDuration;
        [SerializeField] private float _timeGapBetweenResourcesDelivery;

        [Header("Resource selling")]
        [SerializeField] private int _woodUnitsPerCoin;
        [SerializeField] private int _stoneUnitsPerCoin;
        [SerializeField] private int _goldUnitsPerCoin;


        [Header("Bot")]
        [SerializeField] private float _tryToFindResourceAgainTime;


        public float FadeDuration => _fadeDuration;

        public float RestoreTreeAfter => _restoreTreeAfter;

        public float DropoutCollectDuration => _dropoutCollectDuration;

        public float DeliverResourceDuration => _deliverResourceDuration;

        public float TimeGapBetweenResourcesDelivery => _timeGapBetweenResourcesDelivery;

        public float TryToFindResourceAgainTime => _tryToFindResourceAgainTime;
        
        public int GetResourceUnitsPerCoin(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.Wood:
                    return _woodUnitsPerCoin;
                case ResourceType.Stone:
                    return _stoneUnitsPerCoin;
                case ResourceType.Gold:
                    return _goldUnitsPerCoin;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
            }
        }
    }
}