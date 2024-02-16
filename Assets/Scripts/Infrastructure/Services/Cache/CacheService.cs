using Gameplay.Bots.StateMachine.States;
using Gameplay.Buildings;
using Gameplay.Resource;

namespace Infrastructure.Services.Cache
{
    public class CacheService : ICacheService
    {
        public CacheContainer<DropoutResource> ResourceDropout { get; private set; }
        public CacheContainer<Building> Buildings { get; set; }

        public void Initialize()
        {
            InitializeResourceDropout();
            InitializeBuildings();
        }

        private void InitializeResourceDropout() => 
            ResourceDropout = new CacheContainer<DropoutResource>();
        
        private void InitializeBuildings() => 
            Buildings = new CacheContainer<Building>();
    }
}