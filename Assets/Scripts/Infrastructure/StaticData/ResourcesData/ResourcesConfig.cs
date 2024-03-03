using System;
using DG.Tweening;
using Gameplay.Resource;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.StaticData.ResourcesData
{
    [CreateAssetMenu(fileName = "ResourcesConfig", menuName = "StaticData/Configs/ResourcesConfig")]
    public class ResourcesConfig : ScriptableObject
    {
        [SerializeField] private float _fadeDuration;
        [SerializeField] private float _restoreTreeAfter;
        
        [Header("Dropout")]
        [SerializeField] private float _dropoutCollectDuration;
        [SerializeField] private int _minDropoutsPerExtract = 3;
        [SerializeField] private int _maxDropoutsPerExtract = 4;
        [SerializeField] private float _collectScaleTo;

        [Header("Resource sharing")]
        [SerializeField] private float _deliverResourceDuration;
        [SerializeField] private float _timeGapBetweenResourcesDelivery;

        [Header("Resource selling")]
        [SerializeField] private int _woodUnitsPerCoin;
        [SerializeField] private int _stoneUnitsPerCoin;
        [SerializeField] private int _goldUnitsPerCoin;

        [Header("Tree")]
        [SerializeField] private float _treeFadeDuration;
        [SerializeField] private float _fadeDelay;

        [Header("Bot")]
        [SerializeField] private float _tryToFindResourceAgainTime;

        [Header("Animation")]
        [SerializeField] private float _sourceAppearDuration;
        [SerializeField] private AnimationCurve _sourceAppearCurve;

        public float FadeDuration => _fadeDuration;

        public float RestoreTreeAfter => _restoreTreeAfter;

        public float DropoutCollectDuration => _dropoutCollectDuration;

        public float DeliverResourceDuration => _deliverResourceDuration;

        public float TimeGapBetweenResourcesDelivery => _timeGapBetweenResourcesDelivery;

        public float TryToFindResourceAgainTime => _tryToFindResourceAgainTime;

        public float TreeFadeDuration => _treeFadeDuration;

        public float FadeDelay => _fadeDelay;

        public float SourceAppearDuration => _sourceAppearDuration;

        public AnimationCurve SourceAppearCurve => _sourceAppearCurve;
        public int RandomDropoutsPerExtract => Random.Range(_minDropoutsPerExtract, _maxDropoutsPerExtract);

        public float CollectScaleTo => _collectScaleTo;

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