using UnityEngine;

namespace Infrastructure.Services.Pool.Particle
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleRender : MonoBehaviour
    {
        private Release _release;
        private Camera _mainCamera;
        private ParticleSystem _particleSystem;
        private Renderer _renderer;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _renderer = GetComponent<Renderer>();
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnParticleSystemStopped()
        {
            _release.Invoke();
        }

        public void DisableIfNotVisible()
        {
            if (!CheckVisibility())
            {
                _particleSystem.Stop();
                _release.Invoke();
            }
        }

        public void SetRelease(Release release)
        {
            _release = release;
        }

        private bool CheckVisibility()
        {
            Vector3 cameraToObject = transform.position - _mainCamera.transform.position;
            float dot = Vector3.Dot(cameraToObject, _mainCamera.transform.forward);
            
            if (dot < 0 || !IsWithinViewport(transform.position))
            {
                Debug.Log(gameObject.name + " is not visible to the camera.");
                return false;
            }
            else
            {
                Debug.Log(gameObject.name + " is visible to the camera.");
                return true;
            }
        }

        private bool IsWithinViewport(Vector3 worldPosition)
        {
            Vector3 viewportPoint = _mainCamera.WorldToViewportPoint(worldPosition);
            
            return (viewportPoint.x is >= 0 and <= 1 && viewportPoint.y is >= 0 and <= 1 && viewportPoint.z >= 0);
        }
    }
}