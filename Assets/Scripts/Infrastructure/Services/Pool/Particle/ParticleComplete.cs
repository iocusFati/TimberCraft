using System;
using UnityEngine;

namespace Infrastructure.Services.Pool.Particle
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleComplete : MonoBehaviour
    {
        private Release _release;

        private void OnParticleSystemStopped()
        {
            _release.Invoke();
        }

        public void SetRelease(Release release)
        {
            _release = release;
        }
    }
}