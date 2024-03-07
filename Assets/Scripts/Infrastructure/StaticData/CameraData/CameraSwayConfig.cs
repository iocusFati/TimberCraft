using UnityEngine;

namespace Infrastructure.StaticData.CameraData
{
    [CreateAssetMenu(fileName = "CameraSwayConfig", menuName = "StaticData/Configs/CameraSwayConfig")]
    public class CameraSwayConfig : ScriptableObject
    {
        [SerializeField] private float _radius;
        [SerializeField] private bool _negativeSway;
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _swayAnimationCurve;

        public float Radius => _radius;
        public bool NegativeSway => _negativeSway;
        public float Duration => _duration;
        public AnimationCurve SwayAnimationCurve => _swayAnimationCurve;
    }
}