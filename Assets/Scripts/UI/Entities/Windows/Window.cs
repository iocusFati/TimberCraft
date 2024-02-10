using UI.Factory;
using UnityEngine;

namespace UI.Entities.Windows
{
    public class Window : MonoBehaviour, IUIEntity
    {
        public virtual void Show() => 
            gameObject.SetActive(true);

        public virtual void Hide() => 
            gameObject.SetActive(false);
    }
}