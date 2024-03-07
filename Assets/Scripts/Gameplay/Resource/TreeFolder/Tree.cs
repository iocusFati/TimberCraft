using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Gameplay.Player;
using Gameplay.Player.ObstacleFade;
using Infrastructure.Services.Pool;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.ResourcesData;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Resource
{
    public class Tree : ResourceSource
    {
        [Header("Tree")]
        [SerializeField] private MeshRenderer _treeTipMeshRenderer;
        [SerializeField] private Transform _leavesParticleSpawn;
        [SerializeField] private Transform _stump;
        [SerializeField] private TriggerInteraction _stumpTriggerCheck;

        private float _fadeDuration;
        private float _fadeDelay;
        private float _onLandedVFXBaseRadius;
        private float _onLandedVFXTreeTipRadius;
        private float _onLandedVFXSizeModifier;
        private float _onLandedVFXSpeedMultiplier;
        private float _baseOnLandedVFXSpeedModifier;
        private float _onLandedVFXBaseSimulationSpeed;
        private float _onLandedVFXSimulationSpeedMultiplier;
        private Vector2 _onLandedBaseVFXSize;

        private ParticlePool _leavesParticlePool;
        private ParticlePool _onLandedParticlePool;

        private Vector3 _treeTipInitialPosition;

        private FadeObscurePlayerObjects _fadeObscurePlayerObjects;
        private Transform _treeTip;
        private Rigidbody _treeTipRB;
        private Collider _stumpCollider;

        private List<Rigidbody> _segmentForces;
        private readonly Dictionary<Transform, Vector3> _segmentPositions = new();

        [Inject]
        public void Construct(IPoolService poolService,
            IStaticDataService staticData,
            FadeObscurePlayerObjects fadeObscurePlayerObjects)
        {
            _hitParticlePool = poolService.WoodHitParticlesPool;
            _leavesParticlePool = poolService.LeavesParticlesPool;
            _onLandedParticlePool = poolService.OnLandedParticlesPool;
            _logPool = poolService.DropoutsPool[ResourceType.Wood];
            _fadeObscurePlayerObjects = fadeObscurePlayerObjects;

            ResourcesConfig resourcesConfig = staticData.ResourcesConfig;
            _restoreSourceAfter = resourcesConfig.RestoreTreeAfter;
            _fadeDuration = resourcesConfig.TreeFadeDuration;
            _fadeDelay = resourcesConfig.FadeDelay;
            _onLandedVFXSizeModifier = resourcesConfig.OnLandedVFXSizeModifier;
            _onLandedVFXSpeedMultiplier = resourcesConfig.OnLandedVFXSpeedMultiplier;
            _baseOnLandedVFXSpeedModifier = resourcesConfig.OnLandedBaseVFXSpeedModifier;
            _onLandedBaseVFXSize = resourcesConfig.OnLandedBaseVFXSize;
            _onLandedVFXBaseSimulationSpeed = resourcesConfig.OnLandedVFXBaseSimulationSpeed;
            _onLandedVFXSimulationSpeedMultiplier = resourcesConfig.OnLandedVFXSimulationSpeedMultiplier;
            _onLandedVFXBaseRadius = resourcesConfig.OnLandedVFXBaseRadius;
            _onLandedVFXTreeTipRadius = resourcesConfig.OnLandedVFXTreeTipRadius;
        }

        private void Start()
        {
            _treeTip = _treeTipMeshRenderer.transform.parent;
            _treeTipRB = _treeTip.GetComponent<Rigidbody>();
            _stumpCollider = _stumpTriggerCheck.GetComponent<Collider>();

            InitializeInitialPositions();
            
            _stumpCollider.enabled = false;
            _stumpTriggerCheck.OnTriggerEntered += StumpTriggerEnterKickedIn;
        }

        private void StumpTriggerEnterKickedIn(Collider obj)
        {
            if (obj.CompareTag(Tags.Resource) || obj.CompareTag(Tags.TreeTip))
            {
                SpawnLandParticle();
                SpawnLeavesParticle();
            }
        }

        public override void GetDamage(Vector3 hitPoint, Transform hitTransform, out bool resourceSourceDestroyed)
        {
            _stumpCollider.enabled = true;

            base.GetDamage(hitPoint, hitTransform, out resourceSourceDestroyed);
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

            StartCoroutine(ActionAfterDelay(_fadeDelay, TreeTipFadeOut));
        }

        protected override void RestoreSource()
        {
            foreach (var segment in _segmentPositions.Keys) 
                AddSegment(segment);
            
            SetTreeTipToInitialState();
            
            base.RestoreSource();
        }

        private void SpawnLeavesParticle()
        {
            ParticleSystem leavesParticle = _leavesParticlePool.Get();
            leavesParticle.transform.position = _leavesParticleSpawn.position;
        }

        private void SpawnLandParticle()
        {
            ParticleSystem onLandedParticle = _onLandedParticlePool.Get();
            ParticleSystem.MainModule mainModule = onLandedParticle.main;
            ParticleSystem.ShapeModule shapeModule = onLandedParticle.shape;

            onLandedParticle.transform.position = _stump.position;

            mainModule.startSize = new ParticleSystem.MinMaxCurve(
                _onLandedBaseVFXSize.x + _onLandedVFXSizeModifier * _segments.Count,
                _onLandedBaseVFXSize.y + _onLandedVFXSizeModifier * _segments.Count);
            mainModule.startSpeedMultiplier = 
                _baseOnLandedVFXSpeedModifier + _onLandedVFXSpeedMultiplier * _segments.Count;
            mainModule.simulationSpeed =
                _onLandedVFXBaseSimulationSpeed + _onLandedVFXSimulationSpeedMultiplier * _segments.Count;

            if (_segments.Count == 0)
            {
                shapeModule.radius = _onLandedVFXTreeTipRadius;
                _stumpCollider.enabled = false;
            }
            else
            {
                shapeModule.radius = _onLandedVFXBaseRadius;
            }
        }

        private void TreeTipFadeOut()
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