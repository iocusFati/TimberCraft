namespace Infrastructure.States
{
    public class CacheService : ICacheService
    {
        public CacheContainer<DropoutResource> ResourceDropout { get; private set; }

        public void Initialize()
        {
            InitializeResourceDropout();
        }

        private void InitializeResourceDropout() => 
            ResourceDropout = new CacheContainer<DropoutResource>();
    }
}