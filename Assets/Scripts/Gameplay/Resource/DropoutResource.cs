using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Infrastructure.States
{
    public class DropoutResource : MonoBehaviour
    {
        public MMF_Player FeedbackPlayer;
        
        [Header("Randomization")]
        [SerializeField, HorizontalGroup("Randomization")] 
        private float _minRandomizeDropOffset;
        
        [Header(""), SerializeField, HorizontalGroup("Randomization")] 
        private float _maxRandomizeDropOffset;

        [Header("Animation")]
        [SerializeField] private float _dropToHeight;
        [SerializeField] private float _baseDistanceToTargetPosition;
        [FormerlySerializedAs("Target")] [FormerlySerializedAs("_target")] public Transform Destination;

        public void SetTargetPositionFor_MMF_Feedback(Vector3 originPosition) => 
            Destination.position = FindDropPosition(originPosition);

        private Vector3 FindDropPosition(Vector3 originPosition)
        {
            float randomDistanceToDropOffset = Random.Range(_minRandomizeDropOffset, _maxRandomizeDropOffset);
            float distanceToDropPosition = _baseDistanceToTargetPosition + randomDistanceToDropOffset;

            float dropPositionX = Random.Range(0, distanceToDropPosition);
            float dropPositionZ = distanceToDropPosition - dropPositionX;
            
            return originPosition + new Vector3(dropPositionX, _dropToHeight, dropPositionZ);
        }
    }
}