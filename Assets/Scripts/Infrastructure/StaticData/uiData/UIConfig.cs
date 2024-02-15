using UnityEngine;

namespace Infrastructure.StaticData.uiData
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "StaticData/Configs/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        [Header("ResourceCounter")]
        [SerializeField] private float _resourceCounterScaleDuration;
        [SerializeField] private Vector3 _resourceCounterScaleModifier;
        
        [Header("PopUps")]
        [SerializeField] private float _popUpsAppearDuration;
        [SerializeField] private float _popUpsDisappearDuration;
        [SerializeField] private float _popUpsStartScaleModifier;
        [SerializeField] private float _popUpsAppearAmplitude;

        public float ResourceCounterScaleDuration => _resourceCounterScaleDuration;
        public Vector3 ResourceCounterScaleModifier => _resourceCounterScaleModifier;

        public float PopUpsAppearDuration => _popUpsAppearDuration;

        public float PopUpsDisappearDuration => _popUpsDisappearDuration;

        public float PopUpsStartScaleModifier => _popUpsStartScaleModifier;

        public float PopUpsAppearAmplitude => _popUpsAppearAmplitude;
    }
}