using System.Collections.Generic;
using Gameplay.Buildings;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UI.Entities.HUD_Folder;
using UI.Entities.PopUps;
using UI.Entities.Windows;
using UI.Factory;
using UnityEngine;
using Zenject;

namespace UI.Mediator
{
    public class UIMediator : SerializedMonoBehaviour, IUIMediator
    {
        [OdinSerialize] private Dictionary<MinionHut, MinionHutPopUp> _minionHutPopUps;
        [OdinSerialize] private Dictionary<ResourcesShop, ResourcesShopPopUp> _resourcesShopPopUps;
        [OdinSerialize] private MainHousePopUp _mainHousePopUp;

        private IUIHolder _uiContainer;
        private HUD Hud => _uiContainer.Single<HUD>();

        [Inject]
        public void Construct(IUIHolder uiContainer)
        {
            _uiContainer = uiContainer;
        }

        public void SwitchMinionHutPopUp(MinionHut minionHutKey, bool show) => 
            SwitchPopUp(_minionHutPopUps[minionHutKey], show);

        public void SwitchSellResourcesPopUp(ResourcesShop shopKey, bool show) => 
            SwitchPopUp(_resourcesShopPopUps[shopKey], show);

        public void SwitchMainHousePopUp(bool show) => 
            SwitchPopUp(_mainHousePopUp, show);

        public void DisableJoystick(bool disable) => 
            Hud.DisableJoystick(disable);


        private static void SwitchPopUp(Window window, bool show)
        {
            if (show) 
                window.Show();
            else
                window.Hide();
        }
    }
}