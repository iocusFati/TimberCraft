using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UI.Entities.PopUps;
using UI.Entities.Windows;

namespace Gameplay.Locations
{
    public class UIMediator : SerializedMonoBehaviour, IUIMediator
    {
        [OdinSerialize] private Dictionary<MinionHut, MinionHutPopUp> _minionHutPopUs;

        public void SwitchMinionHutPopUp(MinionHut minionHutKey, bool show) => 
            SwitchWindow(_minionHutPopUs[minionHutKey], show);

        private static void SwitchWindow(Window window, bool show)
        {
            if (show) 
                window.Show();
            else
                window.Hide();
        }
    }
}