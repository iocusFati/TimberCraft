using System.Collections.Generic;
using DG.Tweening;
using Infrastructure.Services.Pool;
using Infrastructure.Services.StaticDataService;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Infrastructure.States
{
    public class Tree : ResourceSource
    {
        [SerializeField] private List<Transform> _segments;
        [FormerlySerializedAs("_treeTip")] [SerializeField] private MeshRenderer _treeTipMeshRenderer;

        private IPoolService _poolService;
        private float _fadeDuration;

        private Transform _treeTip;
        private Vector3 _treeTipInitialPosition;
        private readonly Dictionary<Transform, Vector3> _segmentPositions = new();

        protected override float RestoreSourceAfter { get; set; }

        [Inject]
        public void Construct(IPoolService poolService, IStaticDataService staticData)
        {
            _poolService = poolService;

            _fadeDuration = staticData.ResourcesConfig.FadeDuration;
            RestoreSourceAfter = staticData.ResourcesConfig.RestoreTreeAfter;
        }

        private void Start()
        {
            _treeTip = _treeTipMeshRenderer.transform.parent;
            
            InitializeInitialPositions();
        }

        public override void GetDamage(Vector3 hitPoint, Transform hitTransform)
        {
            base.GetDamage(hitPoint, hitTransform);
            
            PlayHitParticle(hitPoint, hitTransform);

            RemoveFirstSegment();

            if (_segments.Count == 0)
            {
                _treeTipMeshRenderer.material
                    .DOFade(0, _fadeDuration)
                    .OnComplete(() => _treeTip.gameObject.SetActive(true));

                StartCoroutine(WaitAndRestoreSource());
            }
        }

        protected override void RestoreSource()
        {
            base.RestoreSource();

            foreach (var segment in _segmentPositions.Keys) 
                AddSegment(segment);
            
            SetTreeTipToInitialState();
        }

        private void SetTreeTipToInitialState()
        {
            _treeTip.gameObject.SetActive(true);
            _treeTip.transform.position = _treeTipInitialPosition;
            _treeTipMeshRenderer.material.DOFade(1, 0);
        }

        private void InitializeSegmentPositionsDictionary()
        {
            foreach (var segment in _segments)
                _segmentPositions.Add(segment, segment.position);
        }

        private void RemoveFirstSegment()
        {
            _segments[0].gameObject.SetActive(false);
            _segments.RemoveAt(0);
        }

        private void InitializeInitialPositions()
        {
            InitializeSegmentPositionsDictionary();
            _treeTipInitialPosition = _treeTip.transform.position;
        }

        private void AddSegment(Transform segment)
        {
            segment.gameObject.SetActive(true);
            _segments.Add(segment);

            SetSegmentToInitialPosition();
            
            void SetSegmentToInitialPosition() => 
                segment.position = _segmentPositions[segment];
        }

        private void PlayHitParticle(Vector3 hitPoint, Transform hitTransform)
        {
            Vector3 firstSegmentPosition = _segments[0].position;

            ParticleSystem particle = _poolService.WoodHitParticlesPool.Get();
            
            particle.transform.position = new Vector3(firstSegmentPosition.x, hitPoint.y, firstSegmentPosition.z);
            particle.transform.rotation = hitTransform.rotation;
        }
    }
}