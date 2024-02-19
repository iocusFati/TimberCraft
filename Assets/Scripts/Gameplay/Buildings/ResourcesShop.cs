using Infrastructure.Services.SaveLoad;
using UI.Mediator;
using Zenject;

namespace Gameplay.Buildings
{
    public class ResourcesShop : Building
    {
        private IUIMediator _uiMediator;

        [Inject]
        public void Construct(IUIMediator uiMediator)
        {
            _uiMediator = uiMediator;
        }

        public override void InteractWithPlayer()
        {
            _uiMediator.SwitchSellResourcesPopUp(this, true);
        }

        public override void StopInteractingWithPlayer()
        {
            _uiMediator.SwitchSellResourcesPopUp(this, false);
        }
    }
}