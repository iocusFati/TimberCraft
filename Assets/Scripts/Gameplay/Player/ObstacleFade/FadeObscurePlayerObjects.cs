using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Infrastructure.Factories;
using Infrastructure.Services.Cache;
using UnityEngine;
using Zenject;

namespace Gameplay.Player.ObstacleFade
{
    public class FadeObscurePlayerObjects : MonoBehaviour
    { 
        [SerializeField]
        private LayerMask LayerMask;
        [SerializeField]
        private float _fadedAlpha = 0.33f;
    
        [SerializeField]
        private float ChecksPerSecond = 10;
        [SerializeField]
        private float _fadeDuration = 1;
    
        [Header("Read Only Data")]
        [SerializeField]
        private List<MeshRenderer> _objectsBlockingView = new();
        
        private Camera _camera;
        private Transform _playerTransform;
        private CacheContainer<MeshRenderer> _obscureViewObjectsCache;

        private readonly Dictionary<MeshRenderer, Tween> _activeTweens = new();
        private readonly HashSet<MeshRenderer> _checkDisabledObjects = new();

        private readonly RaycastHit[] Hits = new RaycastHit[5];

        private GameObject[] HitsObjects => Hits.Select(hit => hit.collider?.gameObject).ToArray();

        [Inject]
        public void Construct(FactoriesHolderService factoriesHolder, ICacheService cacheService)
        {
            _obscureViewObjectsCache = cacheService.ObscureViewObjects;
            
            factoriesHolder.PlayerFactory.OnPlayerCreated += player =>
            {
                _playerTransform = player.transform;
                
                StartCoroutine(CheckForObjects());
            };
        }
        
        private void Start()
        {
            _camera = Camera.main;
        }

        public void DisableCheckFor(MeshRenderer meshRenderer)
        {
            _checkDisabledObjects.Add(meshRenderer);
            
            KillTweenIfActiveExistsFor(meshRenderer);
        }

        public void EnableCheckFor(MeshRenderer meshRenderer) => 
            _checkDisabledObjects.Remove(meshRenderer);

        public void KillTweenIfActiveExistsFor(MeshRenderer meshRenderer)
        {
            if (_objectsBlockingView.Contains(meshRenderer))
            {
                _objectsBlockingView.Remove(meshRenderer);

                if (_activeTweens.Remove(meshRenderer, out var tween)) 
                    tween.Kill();
            }
        }

        private IEnumerator CheckForObjects()
        {
            WaitForSeconds Wait = new WaitForSeconds(1f / ChecksPerSecond);
    
            while (true)
            {
                int hits = Physics.RaycastNonAlloc(_camera.transform.position,
                    (_playerTransform.transform.position - _camera.transform.position).normalized,
                    Hits,
                    Vector3.Distance(_camera.transform.position, _playerTransform.transform.position),
                    LayerMask);
                
                if (hits > 0)
                {
                    for (int i = 0; i < hits; i++)
                    {
                        MeshRenderer obscureMeshRenderer = _obscureViewObjectsCache.Get(Hits[i].collider.gameObject);

                        if (_checkDisabledObjects.Contains(obscureMeshRenderer))
                            continue;

                        if(_activeTweens.TryGetValue(obscureMeshRenderer, out var activeTween))
                        {
                            activeTween.Kill();
                            
                            _activeTweens.Remove(obscureMeshRenderer);
                            _objectsBlockingView.Remove(obscureMeshRenderer);
                        }

                        Tween tween = obscureMeshRenderer.material
                            .DOFade(_fadedAlpha, _fadeDuration)
                            .OnComplete(() => RemoveTween(obscureMeshRenderer));
                        
                        _activeTweens.Add(obscureMeshRenderer, tween);
                        _objectsBlockingView.Add(obscureMeshRenderer);
                    }
                }
    
                FadeObjectsNoLongerBeingHit();
    
                ClearHits();
    
                yield return Wait;
            }
        }

        private void RemoveTween(MeshRenderer meshRenderer, bool killTween = false)
        {
            _activeTweens.Remove(meshRenderer, out Tween tween);

            if (killTween) 
                tween.Kill();
        }

        private void FadeObjectsNoLongerBeingHit()
        {
            MeshRenderer[] notObscuringObjects = NotObscuringObjects().ToArray();
            
            for (int i = 0; i < notObscuringObjects.Length; i++)
            {
                MeshRenderer blockingRenderer = notObscuringObjects[i];
                
                RemoveTween(blockingRenderer, true);

                Tween tween = blockingRenderer.material
                    .DOFade(1, _fadeDuration)
                    .OnComplete(() => _activeTweens.Remove(blockingRenderer));
                _activeTweens.Add(blockingRenderer, tween);

                _objectsBlockingView.Remove(blockingRenderer);
            }
        }

        private IEnumerable<MeshRenderer> NotObscuringObjects() => 
            _objectsBlockingView.Where(blockingRenderer => !HitsObjects.Contains(blockingRenderer.gameObject));

        private void ClearHits()
        {
            RaycastHit hit = new RaycastHit();
            for (int i = 0; i < Hits.Length; i++)
            {
                Hits[i] = hit;
            }
        }
    }
}