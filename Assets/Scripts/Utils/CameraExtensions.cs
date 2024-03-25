using UnityEngine;

namespace Utils
{
    public static class CameraExtensions
    {
        public static bool IsWithinViewport(this Camera camera, Vector3 worldPosition)
        {
            Vector3 viewportPoint = camera.WorldToViewportPoint(worldPosition);
            
            return (viewportPoint.x is >= 0 and <= 1 && viewportPoint.y is >= 0 and <= 1 && viewportPoint.z >= 0);
        }

    }
}