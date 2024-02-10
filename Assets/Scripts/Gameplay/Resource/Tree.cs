using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Player;
using Infrastructure.Services.Pool;
using Infrastructure.Services.StaticDataService;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Resource
{
    public class Tree : ResourceSource
    {
        [FormerlySerializedAs("_treeTip")] [SerializeField] private MeshRenderer _treeTipMeshRenderer;

        private IPoolService _poolService;
        private float _fadeDuration;

        private Transform _treeTip;
        private Vector3 _treeTipInitialPosition;
        private readonly Dictionary<Transform, Vector3> _segmentPositions = new();
        
        [Inject]
        public void Construct(IPoolService poolService, IStaticDataService staticData)
        {
            _poolService = poolService;
            _particlePool = _poolService.WoodHitParticlesPool;
            _dropoutPool = _poolService.LogsPool;
            
            _restoreSourceAfter = staticData.ResourcesConfig.RestoreTreeAfter;
        }

        private void Start()
        {
            _treeTip = _treeTipMeshRenderer.transform.parent;
            
            InitializeInitialPositions();
        }

        protected override void RemoveFirstStage()
        {
            _segments[0].gameObject.SetActive(false);
            _segments.RemoveAt(0);
        }

        protected override void OnLastStageDestroyed()
        {
            base.OnLastStageDestroyed();
            
            _treeTipMeshRenderer.material
                .DOFade(0, _fadeDuration)
                .OnComplete(() => _treeTip.gameObject.SetActive(true));
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
        
        private void InitializeInitialPositions()
        {
            InitializeSegmentPositionsDictionary();
            _treeTipInitialPosition = _treeTip.transform.position;
        }

        // protected override void PlayHitParticle(Vector3 hitPoint, Transform hitTransform)
        // {
        //     Vector3 firstSegmentPosition = _segments[0].position;
        //
        //     ParticleSystem particle = _poolService.WoodHitParticlesPool.Get();
        //     
        //     particle.transform.position = new Vector3(firstSegmentPosition.x, hitPoint.y, firstSegmentPosition.z);
        //     particle.transform.rotation = hitTransform.rotation;
        // }

        private void AddSegment(Transform segment)
        {
            segment.gameObject.SetActive(true);
            _segments.Add(segment);

            SetSegmentToInitialPosition();
            
            void SetSegmentToInitialPosition() => 
                segment.position = _segmentPositions[segment];
        }
    }
}