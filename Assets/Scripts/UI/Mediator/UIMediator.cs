using System.Collections.Generic;
using Gameplay.Buildings;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UI.Entities.PopUps;
using UI.Entities.Windows;
using UnityEngine;

namespace UI.Mediator
{
    public class UIMediator : SerializedMonoBehaviour, IUIMediator
    {
        [OdinSerialize] private Dictionary<MinionHut, MinionHutPopUp> _minionHutPopUps;
        [OdinSerialize] private Dictionary<ResourcesShop, ResourcesShopPopUp> _resourcesShopPopUps;
        [OdinSerialize] private MainHousePopUp _mainHousePopUp;

        public void SwitchMinionHutPopUp(MinionHut minionHutKey, bool show) => 
            SwitchPopUp(_minionHutPopUps[minionHutKey], show);

        public void SwitchSellResourcesPopUp(ResourcesShop shopKey, bool show)
        {
            SwitchPopUp(_resourcesShopPopUps[shopKey], show);
        }

        public void SwitchMainHousePopUp(bool show)
        {
            SwitchPopUp(_mainHousePopUp, show);
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