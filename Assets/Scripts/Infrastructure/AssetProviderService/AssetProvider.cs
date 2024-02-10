using UnityEngine;
using Zenject;

namespace Infrastructure.AssetProviderService
{
    public class AssetProvider : IAssets
    {
        private readonly IInstantiator _instantiator;

        public AssetProvider(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public TCreatable Instantiate<TCreatable>(string path, Transform parent) where TCreatable : Object
        {
            TCreatable prefab = Load<TCreatable>(path);
            return Object.Instantiate(prefab, parent);
        }

        public TCreatable Instantiate<TCreatable>(string path, Vector3 at) where TCreatable : Object
        {
            TCreatable prefab = Load<TCreatable>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public TCreatable Instantiate<TCreatable>(string path) where TCreatable : Object
        {
            TCreatable prefab = Load<TCreatable>(path);
            return Object.Instantiate(prefab);
        }
        
        public TCreatable InstantiateDI<TCreatable>(string path) where TCreatable : Object => 
            _instantiator.InstantiatePrefabResourceForComponent<TCreatable>(path);

        public TCreatable Load<TCreatable>(string path) where TCreatable : Object => 
            Resources.Load<TCreatable>(path);
    }
}