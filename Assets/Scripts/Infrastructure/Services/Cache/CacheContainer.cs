using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.States
{
    public class CacheContainer<TCached> where TCached : MonoBehaviour
    {
        private readonly Dictionary<GameObject, TCached> _cacheDictionary = new();

        public TCached Get(GameObject key)
        {
            if (!_cacheDictionary.TryGetValue(key, out var cachedValue))
            {
                return CacheNewValue(key);
            }
            else
            {
                Debug.Log(cachedValue);
                return cachedValue;
            }
        }

        private TCached CacheNewValue(GameObject key)
        {
            TCached cachedValue = key.GetComponent<TCached>() ?? key.GetComponentInParent<TCached>();

            _cacheDictionary.Add(key, cachedValue);
            
            return cachedValue;
        }
    }
}