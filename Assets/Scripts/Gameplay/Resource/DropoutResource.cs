using System;
using DG.Tweening;
using Infrastructure.Services.Pool;
using Infrastructure.Services.StaticDataService;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Resource
{
    public class DropoutResource : MonoBehaviour
    {
        [Header("Feedbacks")]
        public MMF_Player FeedbackPlayer;
        [SerializeField] private MMF_Player _shareFeedbackPlayer;
        [SerializeField] private string _shareFeedbackPositionName;

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
        
        public ResourceType Type;

        private float _collectDuration;
        private Vector3 _modelInitialScale;
        private Vector3 _initialScale;

        private Release _release;
        private MMF_Position _shareAnimationFeedback;

        public bool IsCollected { get; private set; }
        public int ResourceValue { get; set; }

        [Inject]
        public void Construct(IStaticDataService staticData)
        {
            _collectDuration = staticData.ResourcesConfig.DropoutCollectDuration;
        }

        private void Awake()
        {
            _modelInitialScale = _model.localScale;
            _initialScale = transform.localScale;

            _shareAnimationFeedback = _shareFeedbackPlayer.GetFeedbackOfType<MMF_Position>(_shareFeedbackPositionName);
        }

        public void SetReleaseDelegate(Release release) => 
            _release = release;

        public void SetTargetPositionFor_MMF_Feedback(Vector3 originPosition) => 
            Destination.position = FindDropPosition(originPosition);

        public void Release()
        {
            _release();

            transform.SetParent(null);
            transform.localScale = _initialScale;
            _model.localScale = _modelInitialScale;
            _model.localPosition = Vector3.zero;
            _model.localRotation = Quaternion.identity;
            _collider.enabled = false;

            IsCollected = false;
        }

        public void GetCollectedAndReleasedTo(Transform to)
        {
            transform.DOScale(Vector3.zero, _collectDuration);
            GetCollectedTo(to, Release);
        }

        public void GetCollectedAndKeptTo(Transform to)
        {
            GetCollectedTo(to);
            transform.DOScale(Vector3.one, _collectDuration);
            transform.DOLocalRotate(Vector3.zero, _collectDuration);
        }

        public void PlayShareAnimationTo(Vector3 target,
            PositivityEnum animationDirection,
            Action<DropoutResource> onResourceDelivered)
        {
            Destination.position = target;
            _shareAnimationFeedback.AnimationDirectionX = animationDirection;
            
            _shareFeedbackPlayer.PlayFeedbacks();
            _shareFeedbackPlayer.Events.OnComplete.AddListener(() => onResourceDelivered.Invoke(this));
        }

        private void GetCollectedTo(Transform to, Action onComplete = null)
        {
            transform.SetParent(to);
            
            transform
                .DOLocalMove(Vector3.zero, _collectDuration)
                .OnComplete(() => onComplete?.Invoke());
            
            FeedbackPlayer.StopFeedbacks();
            IsCollected = true;
            _collider.enabled = false;
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