using Gameplay.Buildings;
using Gameplay.Resource;

namespace Infrastructure.Services.Cache
{
    public class CacheService : ICacheService
    {
        public CacheContainer<DropoutResource> ResourceDropout { get; private set; }
        public CacheContainer<Building> Buildings { get; private set; }
        public CacheContainer<ResourceSource> ResourceSources { get; private set; }

        public void Initialize()
        {
            InitializeResourceDropout();
            InitializeBuildings();
            InitializeResourceSources();
        }

        private void InitializeResourceDropout() => 
            ResourceDropout = new CacheContainer<DropoutResource>();
        
        private void InitializeBuildings() => 
            Buildings = new CacheContainer<Building>();
        
        private void InitializeResourceSources() => 
            ResourceSources = new CacheContainer<ResourceSource>();
    }
}