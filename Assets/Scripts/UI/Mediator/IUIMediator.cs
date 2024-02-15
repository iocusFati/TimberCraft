using UnityEngine;

namespace Gameplay.Locations
{
    public interface IUIMediator
    {
        void SwitchMinionHutPopUp(MinionHut minionHutKey, bool show);
    }
}