﻿using System;
using System.Collections.Generic;
using Gameplay.Player;
using Gameplay.Player.Animation;
using Gameplay.Resource;
using Gameplay.Resource.ResourceStorage;
using Infrastructure;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Input;
using Infrastructure.Services.StaticDataService;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Lumberjack
{
    public class LumberjackBase : MonoBehaviour
    {
        protected const string PlayerCameraTag = "PlayerCamera";
        private const string ResourceDropoutTag = "ResourceDropout";

        [SerializeField] private Animator _animator;
        [SerializeField] private TriggerInteraction _triggerInteraction;
        [SerializeField] private TriggerInteraction _lootTrigger;
        [SerializeField] protected LumberjackAxeEnabler _lumberjackAxeEnabler;
        [SerializeField] protected LumberjackAxe _lumberjackAxe;
        [SerializeField] protected Transform _resourceCollector;
        
        [Header("Text spawners")]
        [SerializeField] private bool _spawnFloatingText;
        [SerializeField, ShowIf("_spawnFloatingText")] private MMFloatingTextSpawner _textSpawnerWood;
        [SerializeField, ShowIf("_spawnFloatingText")] private MMFloatingTextSpawner _textSpawnerStone;

        protected LumberjackAnimator _lumberjackAnimator;

        protected ICoroutineRunner _coroutineRunner;
        protected ICacheService _cacheService;
        protected IGameResourceStorage _gameResourceStorage;
        protected CacheContainer<ResourceSource> _resourceSourceCache;

        private ResourceSource _currentlyMinedResourceSource;

        private readonly HashSet<GameObject> _enteredResources = new();

        [Inject]
        public virtual void Construct(IInputService inputService,
            IStaticDataService staticData,
            ICacheService cacheService,
            ICoroutineRunner coroutineRunner, 
            IGameResourceStorage gameResourceStorage)
        {
            _cacheService = cacheService;
            _coroutineRunner = coroutineRunner;
            _gameResourceStorage = gameResourceStorage;

            _resourceSourceCache = cacheService.ResourceSources;
            
            _lumberjackAnimator = new LumberjackAnimator(_animator);
        }

        protected virtual void Start()
        {
            _lumberjackAxeEnabler.Construct(_lumberjackAxe);

            _triggerInteraction.OnTriggerEntered += OnTriggerInteractionEntered;
            _triggerInteraction.OnTriggerStayed += OnTriggerInteractionStayed;
            _triggerInteraction.OnTriggerExited += OnTriggerInteractionExited;

            _lootTrigger.OnTriggerEntered += OnLootTriggerEntered;
            _lootTrigger.OnTriggerExited += OnLootTriggerExited; 
        }

        protected virtual void OnTriggerInteractionEntered(Collider other)
        {
            if (other.CompareTag(Tags.ResourceSource))
            {
                if (_enteredResources.Count == 0) 
                    _lumberjackAnimator.Chop();
                
                _enteredResources.Add(other.gameObject);

                ResourceSource resourceSource = _resourceSourceCache.Get(other.gameObject);

                resourceSource.OnResourceMined += RemoveSource;
            }
        }

        protected virtual void OnTriggerInteractionExited(Collider other)
        {
            if (other.CompareTag(Tags.ResourceSource))
            {
                // ClearInactiveObjects(_enteredResources);

                ResourceSource resourceSource = _resourceSourceCache.Get(other.gameObject);

                if (_enteredResources.Contains(resourceSource.gameObject)) 
                    resourceSource.OnResourceMined -= RemoveSource;
                
                _enteredResources.Remove(other.gameObject);

                StopChoppingIfNoResources();
            }
        }

        protected virtual void OnTriggerInteractionStayed(Collider other)
        {
        }

        private void OnLootTriggerEntered(Collider other)
        {
            if (other.CompareTag(ResourceDropoutTag)) 
                TryCollectDropout(CachedDropout(other));
        }

        private void OnLootTriggerExited(Collider other)
        {
            
        }

        public virtual void StartAxeSwing()
        {
            
        }

        protected virtual void CollectDropout(DropoutResource dropout)
        {
            
        }

        private void RemoveSource(ResourceSource resourceSource)
        {
            _enteredResources.Remove(resourceSource.gameObject);
            
            StopChoppingIfNoResources();
        }

        private void StopChoppingIfNoResources()
        {
            if (_enteredResources.Count == 0) 
                StopChopping();
        }

        private void TryCollectDropout(DropoutResource dropout)
        {
            if (CanCollectDropout(dropout))
                CollectDropout(dropout);
        }

        protected virtual bool CanCollectDropout(DropoutResource dropout) => 
            !dropout.IsCollected;

        protected void OnDropoutCollected(DropoutResource dropoutResource)
        {
            if (_spawnFloatingText)
            {
                SpawnFloatingText(dropoutResource);
            }
        }

        private void SpawnFloatingText(DropoutResource dropoutResource)
        {
            string text = $"+{dropoutResource.ResourceValue}";
            switch (dropoutResource.Type)
            {
                case ResourceType.Wood:
                    _textSpawnerWood.Spawn(text, dropoutResource.transform.position, Vector3.forward);
                    break;
                case ResourceType.Stone:
                    _textSpawnerStone.Spawn(text, dropoutResource.transform.position, Vector3.forward);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private DropoutResource CachedDropout(Component resourceSourceCollider) => 
            _cacheService.ResourceDropout.Get(resourceSourceCollider.gameObject);

        private void StopChopping()
        {
            _lumberjackAnimator.StopChopping();
            _lumberjackAxeEnabler.DisableAxeCollider();
        }
    }
}