using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.Cache
{
    public class CacheContainer<TCached> where TCached : Component
    {
        private readonly Dictionary<GameObject, TCached> _cacheDictionary = new();

        public TCached Get(GameObject key) =>
            !_cacheDictionary.TryGetValue(key, out var cachedValue) 
                ? CacheNewValue(key) 
                : cachedValue;

        private TCached CacheNewValue(GameObject key)
        {
            TCached cachedValue = key.GetComponent<TCached>() ?? key.GetComponentInParent<TCached>();

            _cacheDictionary.Add(key, cachedValue);
            
            return cachedValue;
        }
    }
}