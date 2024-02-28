using Gameplay.Buildings;

namespace UI.Mediator
{
    public interface IUIMediator
    {
        void SwitchMinionHutPopUp(MinionHut minionHutKey, bool show);
        void SwitchSellResourcesPopUp(ResourcesShop shopKey, bool show);
        void SwitchMainHousePopUp(bool show);
        void DisableJoystick(bool disable);
    }
}