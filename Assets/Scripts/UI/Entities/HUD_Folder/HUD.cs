using UI.Entities.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Entities.HUD_Folder
{
    public class HUD : Window 
    {
        [SerializeField] private Image _joystick;

        public void DisableJoystick(bool disable) => 
            _joystick.gameObject.SetActive(!disable);
    } 
}