using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Gameplay.Player.ObstacleFade;
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
        
        private FadeObscurePlayerObjects _fadeObscurePlayerObjects;

        private Transform _treeTip;
        private Rigidbody _treeTipRB;
        private Vector3 _treeTipInitialPosition;
        private readonly Dictionary<Transform, Vector3> _segmentPositions = new();
        private List<Rigidbody> _segmentForces;

        [Inject]
        public void Construct(IPoolService poolService,
            IStaticDataService staticData,
            FadeObscurePlayerObjects fadeObscurePlayerObjects)
        {
            _particlePool = poolService.WoodHitParticlesPool;
            _logPool = poolService.DropoutsPool[ResourceType.Wood];
            _fadeObscurePlayerObjects = fadeObscurePlayerObjects;

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

            StartCoroutine(ActionAfterDelay(_fadeDelay, FadeOut));
        }

        protected override void RestoreSource()
        {
            foreach (var segment in _segmentPositions.Keys) 
                AddSegment(segment);
            
            SetTreeTipToInitialState();
            
            base.RestoreSource();
        }

        private void FadeOut()
        {
            _fadeObscurePlayerObjects.DisableCheckFor(_treeTipMeshRenderer);
            
            _treeTipMeshRenderer.material
                .DOFade(0, _fadeDuration)
                .OnComplete(() =>
                {
                    _treeTip.gameObject.SetActive(false);
                    _fadeObscurePlayerObjects.EnableCheckFor(_treeTipMeshRenderer);
                });
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
        
        private void AddSegment(Transform segment)
        {
            segment.gameObject.SetActive(true);
            _segments.Add(segment);

            SetSegmentToInitialPosition();
            
            void SetSegmentToInitialPosition() => 
                segment.position = _segmentPositions[segment];
        }

        private IEnumerator ActionAfterDelay(float delay, Action fadeOut)
        {
            yield return new WaitForSeconds(delay);
            
            fadeOut.Invoke();
        }
    }
}