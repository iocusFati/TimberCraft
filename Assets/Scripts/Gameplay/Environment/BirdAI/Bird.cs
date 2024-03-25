using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using UniRx;
using UnityEngine;
using Utils;

namespace Gameplay.Environment.BirdAI
{
    public class Bird : MonoBehaviour
    {
        [SerializeField] private float _flyAwayDistance;
        [SerializeField] private Transform _flyTarget;
        [SerializeField] private ParticleSystem _flyFx;
        
        [Header("Feedbacks")]
        [SerializeField] private MMF_Player _flyFeedback;
        [SerializeField] private MMF_Player _getScaredFeedback;
        [SerializeField] private MMF_Player _setIdleFeedback;

        [Header("Presets")]
        [SerializeField] private List<BirdFlyPreset> _flyTargetPresets;

        [Header("Custom Trigger")]
        [SerializeField] private bool _hasCustomTrigger;
        
        
        private Transform _cameraTransform;
        private Camera _camera;

        private ICustomTrigger _customTrigger;
        private CompositeDisposable _flyDisposer;
        private MMF_Position _positionFeedback;

        private Vector3 _startPosition;
        private Quaternion _startRotation;
        public ReactiveProperty<bool> HasFlownAway { get; } = new();

        private void Awake()
        {
            _camera = Camera.main;
            _cameraTransform = _camera.transform;

            _flyTarget.SetParent(_cameraTransform);
            
            SetUpFeedbacks();

            _flyFx.transform.SetParent(null);
            
            _startPosition = transform.position;
            _startRotation = transform.rotation;
            
            _flyDisposer = new CompositeDisposable();

            if (_hasCustomTrigger) 
                _customTrigger = GetComponent<ICustomTrigger>();

            InitializeAsync().Forget();
        }

        private void SetUpFeedbacks()
        {
            MMF_Rotation rotationFeedback = _flyFeedback.GetFeedbackOfType<MMF_Rotation>();
            rotationFeedback.Destination = _cameraTransform;
            
            _positionFeedback = _flyFeedback.GetFeedbackOfType<MMF_Position>();
            _positionFeedback.DestinationPositionTransform = _flyTarget;
        }

        public async UniTaskVoid InitializeAsync()
        {
            await UniTask.WaitUntil(() => !_camera.IsWithinViewport(_startPosition));


            gameObject.SetActive(true);
            
            SetIdle();

            transform.position = _startPosition;
            transform.rotation = _startRotation;

            HasFlownAway.Value = false;

            if (_hasCustomTrigger) 
                FlyAwayOnCustomTriggerAsync().Forget();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasCustomTrigger)
                return;
            
            if (CollisionIsPlayer(other))
            {
                GetScared();

                SubscribeToPlayerOnClose(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (CollisionIsPlayer(other))
            {
                SetIdle();
                
                _flyDisposer.Clear();
            }
        }

        private async UniTaskVoid FlyAwayOnCustomTriggerAsync()
        {
            if (_customTrigger is not null)
            {
                await _customTrigger.WaitForTriggerAsync();
                
                FlyAway();
            }
        }

        private void SubscribeToPlayerOnClose(Collider other) =>
            Observable.EveryUpdate()
                .Where(_ => PlayerIsCloseEnough(other))
                .First()
                .Subscribe(_ => FlyAway())
                .AddTo(_flyDisposer);

        private bool PlayerIsCloseEnough(Collider player)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            bool playerIsCloseEnough = distanceToPlayer <= _flyAwayDistance;
            return playerIsCloseEnough;
        }

        private void SetIdle() => 
            _setIdleFeedback.PlayFeedbacks();

        private void GetScared() => 
            _getScaredFeedback.PlayFeedbacks();

        private void FlyAway()
        {
            _flyFeedback.PlayFeedbacks();
            _flyFeedback.Events.OnComplete.AddListener(OnFinishedFlying);

            SetAccordingRandomPreset();

            _flyDisposer.Clear();

            HasFlownAway.Value = true;
        }

        private void OnFinishedFlying()
        {
            gameObject.SetActive(false);
            
            _flyFeedback.Events.OnComplete.RemoveListener(OnFinishedFlying);
        }

        private void SetAccordingRandomPreset()
        {
            BirdFlyPreset preset = _flyTargetPresets.GetRandom();
            _flyTarget.localPosition = preset.Offset;
            _positionFeedback.AnimationDirectionX = preset.Direction;
        }

        private bool CollisionIsPlayer(Collider other) => 
            other.gameObject.CompareTag(Tags.Player);
    }

    [Serializable]
    public struct BirdFlyPreset
    {
        public Vector3 Offset;
        public PositivityEnum Direction;
    }
}