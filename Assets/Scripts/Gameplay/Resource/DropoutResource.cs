using Infrastructure.Services.Pool;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.States
{
    public class DropoutResource : MonoBehaviour
    {
        [Header("Feedbacks")]
        public MMF_Player FeedbackPlayer;
        [SerializeField] private MMF_Player _getCollectedByLumberjackFeedbackPlayer;

        [Header("Randomization")]
        [SerializeField, HorizontalGroup("Randomization")] 
        private float _minRandomizeDropOffset;

        [Header(""), SerializeField, HorizontalGroup("Randomization")] 
        private float _maxRandomizeDropOffset;

        [Header("Animation")]
        [SerializeField] private float _dropToHeight;
        [SerializeField] private float _baseDistanceToTargetPosition;
        public Transform Destination;

        [Header("Other")] 
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _model;

        private Release _release;

        public void SetReleaseDelegate(Release release) => 
            _release = release;

        public void SetTargetPositionFor_MMF_Feedback(Vector3 originPosition) => 
            Destination.position = FindDropPosition(originPosition);

        public void GetCollectedTo(Vector3 to)
        {
            // if (storage.IsFull) return;
            //
            // Vector3 freePosition = storage.GetFreePosition();
            // storage.OccupyFreePosition();
            
            Destination.position = to;
                
            _getCollectedByLumberjackFeedbackPlayer.PlayFeedbacks();
            FeedbackPlayer.StopFeedbacks();
        }

        public void Release()
        {
            _release();
            
            _collider.enabled = false;
            _model.localScale = Vector3.one;
            _model.localPosition = Vector3.zero;
        }

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