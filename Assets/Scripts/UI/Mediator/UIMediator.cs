using System.Collections.Generic;
using Gameplay.Buildings;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UI.Entities.PopUps;
using UI.Entities.Windows;

namespace UI.Mediator
{
    public class UIMediator : SerializedMonoBehaviour, IUIMediator
    {
        [OdinSerialize] private Dictionary<MinionHut, MinionHutPopUp> _minionHutPopUps;
        [OdinSerialize] private Dictionary<ResourcesShop, ResourcesShopPopUp> _resourcesShopPopUps;

        public void SwitchMinionHutPopUp(MinionHut minionHutKey, bool show) => 
            SwitchPopUp(_minionHutPopUps[minionHutKey], show);

        public void SwitchSellResourcesPopUp(ResourcesShop shopKey, bool show)
        {
            SwitchPopUp(_resourcesShopPopUps[shopKey], show);
        }

        private static void SwitchPopUp(Window window, bool show)
        {
            if (show) 
                window.Show();
            else
                window.Hide();
        }
    }
}