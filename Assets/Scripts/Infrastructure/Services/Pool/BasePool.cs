﻿using System.Collections.Generic;
using Infrastructure.AssetProviderService;
using UnityEngine;
using UnityEngine.Pool;

namespace Infrastructure.Services.Pool
{
    public delegate void Release();
    
    public abstract class BasePool<TPoolable> where TPoolable : Component
    {
        protected readonly IAssets _assets;
        
        private IObjectPool<TPoolable> _pool;
        
        private readonly List<TPoolable> _activePoolables = new();

        private IObjectPool<TPoolable> Pool
        {
            get
            {
                return _pool ??= new ObjectPool<TPoolable>(
                    Spawn,
                    poolable => { poolable.gameObject.SetActive(true); }, 
                    poolable => { poolable.gameObject.SetActive(false); },
                    poolable => { Object.Destroy(poolable.gameObject); });
            }
        }

        protected BasePool(IAssets assets)
        {
            _assets = assets;
        }

        public void Initialize(int initialSpawnCount = 5)
        {
            for (int i = 0; i < initialSpawnCount; i++)
            {
                Get();
            }
            
            ReleaseAll();
        }

        protected abstract TPoolable Spawn();

        public virtual TPoolable Get()
        {
            TPoolable poolable = Pool.Get();
            _activePoolables.Add(poolable);

            return poolable;
        }

        public virtual void Release(TPoolable poolable)
        {
            if (!_activePoolables.Contains(poolable))
                return;
            
            Pool.Release(poolable);
            _activePoolables.Remove(poolable);
        }

        public void ReleaseAll()
        {
            foreach (var poolable in _activePoolables)
            {
                ReleaseWithoutRemove(poolable);
            }            
            _activePoolables.Clear();
        }

        private void ReleaseWithoutRemove(TPoolable poolable) => 
            _pool.Release(poolable);
    }
}