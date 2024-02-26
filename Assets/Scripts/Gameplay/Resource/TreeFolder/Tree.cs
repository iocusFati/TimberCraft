using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Infrastructure.Services.Pool;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.ResourcesData;
using UnityEngine;
using Zenject;

namespace Gameplay.Resource
{
    public class Tree : ResourceSource
    {
        [SerializeField] private MeshRenderer _treeTipMeshRenderer;

        private float _fadeDuration;
        private float _fadeDelay;

        private Transform _treeTip;
        private Rigidbody _treeTipRB;
        private Vector3 _treeTipInitialPosition;
        private readonly Dictionary<Transform, Vector3> _segmentPositions = new();
        private List<Rigidbody> _segmentForces;

        [Inject]
        public void Construct(IPoolService poolService, IStaticDataService staticData)
        {
            _particlePool = poolService.WoodHitParticlesPool;
            _logPool = poolService.DropoutsPool[ResourceType.Wood];

            ResourcesConfig resourcesConfig = staticData.ResourcesConfig;
            _restoreSourceAfter = resourcesConfig.RestoreTreeAfter;
            _fadeDuration = resourcesConfig.TreeFadeDuration;
            _fadeDelay = resourcesConfig.FadeDelay;
        }

        private void Start()
        {
            _treeTip = _treeTipMeshRenderer.transform.parent;
            _treeTipRB = _treeTip.GetComponent<Rigidbody>();
            
            InitializeInitialPositions();
        }

        public void SetKinematic(bool kinematic)
        {
            foreach (var segment in _segmentForces) 
                segment.isKinematic = kinematic;

            _treeTipRB.isKinematic = kinematic;
        }

        protected override void OnLastStageDestroyed()
        {
            base.OnLastStageDestroyed();
            
            _treeTipMeshRenderer.material
                .DOFade(0, _fadeDuration)
                .SetDelay(_fadeDelay)
                .OnComplete(() => _treeTip.gameObject.SetActive(false));
        }

        protected override void RestoreSource()
        {
            foreach (var segment in _segmentPositions.Keys) 
                AddSegment(segment);
            
            SetTreeTipToInitialState();
            
            base.RestoreSource();
        }

        private void SetTreeTipToInitialState()
        {
            _treeTip.gameObject.SetActive(true);
            _treeTip.transform.localPosition = _treeTipInitialPosition;
            _treeTipMeshRenderer.material.DOFade(1, 0);
        }

        private void InitializeSegmentPositionsDictionary()
        {
            foreach (var segment in _segments)
                _segmentPositions.Add(segment, segment.position);
            
            _segmentForces = _segmentPositions.Keys
                .Select(segment => segment.GetComponent<Rigidbody>())
                .ToList();
        }
        
        private void InitializeInitialPositions()
        {
            InitializeSegmentPositionsDictionary();
            _treeTipInitialPosition = _treeTip.transform.localPosition;
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