using Gameplay.Resource;
using Infrastructure.Services.StaticDataService;
using Zenject;

namespace UI.Buttons
{
    public class BuyButtonDisabler : ButtonDisabler
    {
        private IResourcesSelling _resourcesSelling;
        private int _resourceUnitsPerCoin;

        [Inject]
        public void Construct(IStaticDataService staticData, IResourcesSelling resourcesSelling)
        {
            _resourcesSelling = resourcesSelling;
            
            _resourceUnitsPerCoin = staticData.ResourcesConfig.GetResourceUnitsPerCoin(_resourceType);
        }
        
        protected override bool CanBuy(int newResourceCount)
        {
            int sellResourceCount = 
                _resourcesSelling.GetSellResourceCount(_resourceType, _resourceUnitsPerCoin, out _);

            return sellResourceCount != 0;
        }
    }
}