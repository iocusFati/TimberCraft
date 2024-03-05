using System;
using UnityEngine;

namespace Gameplay.Player
{
    public class TriggerInteraction : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEntered;
        public event Action<Collider> OnTriggerExited;
        public event Action<Collider> OnTriggerStayed;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEntered?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExited?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayed?.Invoke(other);
        }
    }
}