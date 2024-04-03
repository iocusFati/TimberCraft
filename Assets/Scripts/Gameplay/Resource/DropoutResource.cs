using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Services.Pool;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.ResourcesData;
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
        private float _collectScaleTo;
        private Vector3 _modelInitialScale;
        private Vector3 _initialScale;

        private Release _release;
        private MMF_Position _shareAnimationFeedback;
        private ResourcesConfig _resourcesConfig;
        private CancellationTokenSource _fadeOutCancellationSource;
        private Quaternion _initialModelRotation;

        public bool IsCollected { get; private set; }
        public int ResourceValue { get; set; }

        [Inject]
        public void Construct(IStaticDataService staticData)
        {
            _resourcesConfig = staticData.ResourcesConfig;
            
            _collectDuration = _resourcesConfig.DropoutCollectDuration;
            _collectScaleTo = _resourcesConfig.CollectScaleTo;
        }

        private void Awake()
        {
            _fadeOutCancellationSource = new CancellationTokenSource();
            
            _modelInitialScale = _model.localScale;
            _initialScale = transform.localScale;
            _initialModelRotation = _model.rotation;

            _shareAnimationFeedback = _shareFeedbackPlayer.GetFeedbackOfType<MMF_Position>(_shareFeedbackPositionName);
        }

        public async UniTaskVoid SetReleaseTimer() => 
            await ReleaseSelfIfNotCollectedInTime();

        private async UniTask ReleaseSelfIfNotCollectedInTime()
        {
            _fadeOutCancellationSource = new CancellationTokenSource();
            
            await UniTask.Delay(TimeSpan.FromSeconds(_resourcesConfig.DropoutFadeOutTime),
                cancellationToken: _fadeOutCancellationSource.Token);

            await UniTask.WhenAll(
                transform.DOScale(0, _resourcesConfig.DropoutFadeOutDuration)
                    .WithCancellation(_fadeOutCancellationSource.Token));
            
            Release();
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
            _model.localRotation = _initialModelRotation;
            _collider.enabled = false;

            IsCollected = false;
        }

        public void GetCollectedAndReleasedTo(Transform to, Action<DropoutResource> OnCollected)
        {
            transform.DOScale(_collectScaleTo, _collectDuration).SetEase(Ease.InCubic);
            
            GetCollectedTo(to, () =>
            {
                Release();
                OnCollected.Invoke(this);
            });
        }

        public void GetCollectedAndKeptTo(Transform to)
        {
            GetCollectedTo(to);
            transform.DOScale(Vector3.one, _collectDuration);
            transform.DOLocalRotate(Vector3.zero, _collectDuration);
        }

        public async UniTask PlayShareAnimationTo(Vector3 target,
            PositivityEnum animationDirection)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            Destination.position = target;
            _shareAnimationFeedback.AnimationDirectionX = animationDirection;

            _shareFeedbackPlayer.PlayFeedbacks();
            
            await _shareFeedbackPlayer.Events.OnComplete.OnInvokeAsync(token);
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
            
            _fadeOutCancellationSource.Cancel();
        }

        private Vector3 FindDropPosition(Vector3 originPosition)
        {
            float randomDistanceToDropOffset = Random.Range(_minRandomizeDropOffset, _maxRandomizeDropOffset);
            float distanceToDropPosition = _baseDistanceToTargetPosition + randomDistanceToDropOffset;

            float dropPositionX = Random.Range(0, distanceToDropPosition);
            float dropPositionZ = distanceToDropPosition - dropPositionX;

            int randomSign = Random.Range(0, 2) * 2 - 1; ;
            
            return originPosition + new Vector3(dropPositionX * randomSign, _dropToHeight, dropPositionZ);
        }
    }
}