using UnityEngine;

namespace Infrastructure.StaticData.ResourcesData
{
    [CreateAssetMenu(fileName = "TreeCreateConfig", menuName = "StaticData/Configs/TreeCreateConfig")]
    public class TreeCreateConfig : ScriptableObject
    {
        [SerializeField] private float _forcePerSegment;
        [SerializeField] private float _maxMass;
        [SerializeField] private float _massPerSegment;
        [SerializeField] private float _treeTipMass;

        public float ForcePerSegment => _forcePerSegment;

        public float MaxMass => _maxMass;

        public float MassPerSegment => _massPerSegment;

        public float TreeTipMass => _treeTipMass;
    }
}