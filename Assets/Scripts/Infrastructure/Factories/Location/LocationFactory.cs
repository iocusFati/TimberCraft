using Gameplay.Locations;
using Infrastructure.AssetProviderService;
using Utils;

namespace Infrastructure.Factories.Location
{
    public class LocationFactory
    {
        private readonly IAssets _assets;

        public LocationFactory(IAssets assets)
        {
            _assets = assets;
        }

        public MainLocation CreateMainLocation()
        {
            MainLocation location = _assets.Instantiate<MainLocation>(AssetPaths.MainLocation);

            return location;
        }
    }
}