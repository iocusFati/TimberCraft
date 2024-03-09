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
        private float ChecksPerSecond = 10;
    
        [Header("Read Only Data")]
        private readonly List<IObscurablePlayer> _objectsBlockingView = new();
        
        private Camera _camera;
        private Transform _playerTransform;
        private CacheContainer<IObscurablePlayer> _obscureViewObjectsCache;

        private readonly Dictionary<IObscurablePlayer, Tween> _activeFadeOutTweens = new();
        private readonly Dictionary<IObscurablePlayer, Tween> _activeFadeInTweens = new();
        private readonly HashSet<IObscurablePlayer> _checkDisabledObjects = new();

        private readonly RaycastHit[] Hits = new RaycastHit[5];

        private static readonly int Alpha = Shader.PropertyToID("_Alpha");

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

        public void DisableCheckFor(IObscurablePlayer obscurable)
        {
            _checkDisabledObjects.Add(obscurable);
            
            obscurable.MainGO.SetActive(true);
            obscurable.DitherGO.SetActive(false);
            
            // KillTweenIfActiveExistsFor(obscurable);
        }

        public void EnableCheckFor(IObscurablePlayer obscurable) => 
            _checkDisabledObjects.Remove(obscurable);

        private void KillTweenIfActiveExistsFor(IObscurablePlayer obscurable)
        {
            if (_objectsBlockingView.Contains(obscurable))
            {
                _objectsBlockingView.Remove(obscurable);

                if (_activeFadeOutTweens.Remove(obscurable, out var tween)) 
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
                        IObscurablePlayer obscurable = _obscureViewObjectsCache.Get(Hits[i].collider.gameObject);

                        if (_checkDisabledObjects.Contains(obscurable) || _objectsBlockingView.Contains(obscurable))
                            continue;

                        DitherObscurable(obscurable);
                    }
                }
    
                FadeObjectsNoLongerBeingHit();
    
                ClearHits();
    
                yield return Wait;
            }
        }

        private void DitherObscurable(IObscurablePlayer obscurable)
        {
            obscurable.MainGO.SetActive(false);
            obscurable.DitherGO.SetActive(true);
            // Tween tween = ChangeAlpha(obscurable, _activeFadeOutTweens, _fadedAlpha);

            // Tween tween = obscurable.Meshmaterial
            //     .DOFade(_fadedAlpha, _fadeDuration)
            //     .OnComplete(() => RemoveTween(obscurable));
            
            _objectsBlockingView.Add(obscurable);
        }

        private void FadeObjectsNoLongerBeingHit()
        {
            IObscurablePlayer[] notObscuringObjects = NotObscuringObjects().ToArray();
            
            for (int i = 0; i < notObscuringObjects.Length; i++)
            {
                // if (_activeFadeInTweens.ContainsKey(notObscuringObjects[i]))
                //     continue;
                
                IObscurablePlayer obscurable = notObscuringObjects[i];
                
                obscurable.MainGO.SetActive(true);
                obscurable.DitherGO.SetActive(false);
                
                // RemoveTween(obscurable, _activeFadeOutTweens, true);

                // ChangeAlpha(obscurable, _activeFadeInTweens, 0);
                
                // Tween tween = obscurable.material
                //     .DOFade(1, _fadeDuration)
                //     .OnComplete(() => _activeTweens.Remove(obscurable));

                _objectsBlockingView.Remove(obscurable);
            }
        }

        private IEnumerable<IObscurablePlayer> NotObscuringObjects() => 
            _objectsBlockingView.Where(blockingRenderer => 
                !HitsObjects.Contains(blockingRenderer.BlockerGO));

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