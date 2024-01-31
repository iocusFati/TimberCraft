using UnityEngine;

namespace Infrastructure.StaticData.ResourcesData
{
    [CreateAssetMenu(fileName = "ResourcesConfig", menuName = "StaticData/Configs/ResourcesConfig")]
    public class ResourcesConfig : ScriptableObject
    {
        [SerializeField] private float _fadeDuration;
        [SerializeField] private float _restoreTreeAfter;

        public float FadeDuration => _fadeDuration;

        public float RestoreTreeAfter => _restoreTreeAfter;
    }
}