using System;
using Gameplay.Resource;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.StaticData.ResourcesData
{
    [CreateAssetMenu(fileName = "ResourcesConfig", menuName = "StaticData/Configs/ResourcesConfig")]
    public class ResourcesConfig : ScriptableObject
    {
        [SerializeField, FoldoutGroup("General")] private float _restoreSourceAfter;
        
        [SerializeField, FoldoutGroup("Dropout")] private float _dropoutCollectDuration;
        [SerializeField, FoldoutGroup("Dropout")] private int _minDropoutsPerExtract = 3;
        [SerializeField, FoldoutGroup("Dropout")] private int _maxDropoutsPerExtract = 4;
        [SerializeField, FoldoutGroup("Dropout")] private float _collectScaleTo;
        [SerializeField, FoldoutGroup("Dropout")] private float _dropoutFadeOutTime;
        [SerializeField, FoldoutGroup("Dropout")] private float _dropoutFadeOutDuration;

        
        [SerializeField, FoldoutGroup("Resource share")] private float _deliverResourceDuration;
        [SerializeField, FoldoutGroup("Resource share")] private float _timeGapBetweenResourcesDelivery;
        
        [SerializeField, FoldoutGroup("Resource selling")] private int _woodUnitsPerCoin;
        [SerializeField, FoldoutGroup("Resource selling")] private int _stoneUnitsPerCoin;
        [SerializeField, FoldoutGroup("Resource selling")] private int _goldUnitsPerCoin;
        [SerializeField, FoldoutGroup("Resource selling")] private float _sellResourcesSpeedModifier;

        [SerializeField, FoldoutGroup("Tree")] private float _treeFadeDuration;
        [SerializeField, FoldoutGroup("Tree")] private float _fadeDelay;
        [SerializeField, FoldoutGroup("Tree")] private float _activeImpulseSourceDistance;
        
        [Title("On landed particle", TitleAlignment = TitleAlignments.Centered), Space]
        [Title("Speed", HorizontalLine = false)]
        [SerializeField, FoldoutGroup("Tree")] private float _onLandedBaseVFXSpeedModifier;
        [SerializeField, FoldoutGroup("Tree")] private float _onLandedVFXSpeedMultiplier;
        
        [Header("Simulation speed")]
        [SerializeField, FoldoutGroup("Tree")] private float _onLandedVFXBaseSimulationSpeed;
        [SerializeField, FoldoutGroup("Tree")] private float _onLandedVFXSimulationSpeedMultiplier;
        
        [Header("Radius")]
        [SerializeField, FoldoutGroup("Tree")] private float _onLandedVFXBaseRadius;
        [SerializeField, FoldoutGroup("Tree")] private float _onLandedVFXTreeTipRadius;

        [Header("Size")]
        [SerializeField, FoldoutGroup("Tree")] private Vector2 _onLandedBaseVFXSize;
        [SerializeField, FoldoutGroup("Tree"), Tooltip("More wood piles fall larger size"), MaxValue(1)] 
        private float _onLandedVFXSizeModifier;

        [SerializeField, FoldoutGroup("Bot")] private float _tryToFindResourceAgainTime;

        public float RestoreSourceAfter => _restoreSourceAfter;

        public float DropoutCollectDuration => _dropoutCollectDuration;

        public float DeliverResourceDuration => _deliverResourceDuration;

        public float TimeGapBetweenResourcesDelivery => _timeGapBetweenResourcesDelivery;

        public float TryToFindResourceAgainTime => _tryToFindResourceAgainTime;

        public float TreeFadeDuration => _treeFadeDuration;

        public float FadeDelay => _fadeDelay;

        public int RandomDropoutsPerExtract => Random.Range(_minDropoutsPerExtract, _maxDropoutsPerExtract);

        public float CollectScaleTo => _collectScaleTo;

        public float OnLandedVFXSizeModifier => _onLandedVFXSizeModifier;

        public Vector2 OnLandedBaseVFXSize => _onLandedBaseVFXSize;

        public float OnLandedVFXSpeedMultiplier => _onLandedVFXSpeedMultiplier;

        public float OnLandedBaseVFXSpeedModifier => _onLandedBaseVFXSpeedModifier;

        public float OnLandedVFXBaseSimulationSpeed => _onLandedVFXBaseSimulationSpeed;

        public float OnLandedVFXSimulationSpeedMultiplier => _onLandedVFXSimulationSpeedMultiplier;

        public float OnLandedVFXBaseRadius => _onLandedVFXBaseRadius;

        public float OnLandedVFXTreeTipRadius => _onLandedVFXTreeTipRadius;

        public float ActiveImpulseSourceDistance => _activeImpulseSourceDistance;

        public float SellResourcesSpeedModifier => _sellResourcesSpeedModifier;

        public float DropoutFadeOutTime => _dropoutFadeOutTime;

        public float DropoutFadeOutDuration => _dropoutFadeOutDuration;

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