using System;
using System.Collections;
using Infrastructure;
using Infrastructure.StaticData.CameraData;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Buildings
{
    public class CameraSway : MonoBehaviour
    {
        [SerializeField] private Transform _lookAt;
        [SerializeField] private Transform _lookAtAnchor;
        [SerializeField] private CameraSwayConfig _swayConfig;

        private ICoroutineRunner _coroutineRunner;

        private Camera _camera;

        [Inject]
        public void Construct(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Awake()
        {
            _camera = Camera.main;
        }

        public void Sway()
        {
            _coroutineRunner.StartCoroutine(DoSway());
        }

        private IEnumerator DoSway()
        {
            float time = 0;

            // double minZ = LookAtPositionZ(-_swayConfig.Radius, _swayConfig.Radius);
            // double maxZ = LookAtPositionZ(_swayConfig.Radius, _swayConfig.Radius);
            //
            // Vector3 minPosition = _lookAtAnchor.TransformPoint(
            //     new Vector3(-_swayConfig.Radius, _lookAtAnchor.position.y, (float)minZ));
            // Vector3 maxPosition = _lookAtAnchor.TransformPoint(
            //     new Vector3(_swayConfig.Radius, _lookAtAnchor.position.y, (float)maxZ));
            //
            // double distanceFromCameraToMin = Vector3.Distance(_camera.transform.position, minPosition);
            // double distanceFromCameraToMax = Vector3.Distance(_camera.transform.position, maxPosition);
            //
            // double angleCameraVectors = AngleBetweenCameraVectors(minPosition, maxPosition);
            //
            // double pow = Math.Pow(distanceFromCameraToMin, 2);
            // double pow2 = Math.Pow(distanceFromCameraToMax, 2);
            // double fromCameraToMin = 2 * distanceFromCameraToMin * distanceFromCameraToMax;
            // double f = Math.Cos(angleCameraVectors * Mathf.Deg2Rad);
            // double cos = fromCameraToMin *
            //              f;
            // var distanceBetweenCameraVectors = pow +
            //                                    pow2 -
            //                                    cos;
            //
            // var currentRadius = distanceBetweenCameraVectors / 2;
            //
            // float radius;
            // if (currentRadius < _swayConfig.Radius)
            // {
            //     float radiusDifference = (float)(_swayConfig.Radius - currentRadius);
            //
            //     Vector3 newMinPosition = AddLengthToVector(minPosition, radiusDifference);
            //     Vector3 newMaxPosition = AddLengthToVector(maxPosition, radiusDifference);
            //
            //     radius = Vector3.Distance(newMinPosition, newMaxPosition) * 0.5f;
            //     
            //     Debug.Log("Old radius is: " + currentRadius);
            //     Debug.Log("New radius is: " + radius);
            // }
            // else
            // {
            //     radius = _swayConfig.Radius;
            // }

            while (time < _swayConfig.Duration)
            {
                float lookAtPositionX = LookAtPositionX(time, _swayConfig.Radius);
                double lookAtPositionZ = LookAtPositionZ(lookAtPositionX, _swayConfig.Radius);

                float lookAtPositionZ_F = !double.IsNaN(lookAtPositionZ) ? (float)lookAtPositionZ : 0;

                _lookAt.localPosition = new Vector3(lookAtPositionX, 0, lookAtPositionZ_F);

                time += Time.deltaTime;

                yield return null;
            }
        }

        private float AngleBetweenCameraVectors(Vector3 minPosition, Vector3 maxPosition)
        {
            Vector3 cameraToMinVector = _camera.transform.position - minPosition;
            Vector3 cameraToMaxVector = _camera.transform.position - maxPosition;

            float angleCameraVectors = Vector3.Angle(cameraToMinVector, cameraToMaxVector);
            return angleCameraVectors;
        }

        private float LookAtPositionX(float time, float radius)
        {
            float lookAtPositionX = _swayConfig.SwayAnimationCurve.Evaluate(time) * radius;
            
            lookAtPositionX *= -_swayConfig.NegativeSway.ToPositiveNegativeInt();
            
            return lookAtPositionX;
        }

        private double LookAtPositionZ(float lookAtPositionX, float radius)
        {
            double radiusSquare = Mathf.Pow(radius, 2);
            double xSquare = MathF.Pow(lookAtPositionX, 2);
            double ySquare = 0;

            
            
            double lookAtPositionZ = -_swayConfig.NegativeSway.ToPositiveNegativeInt() * 
                Math.Pow(lookAtPositionX, 3);
            return lookAtPositionZ;
        }

        private Vector3 AddLengthToVector(Vector3 vector, float addedLength)
        {
            Vector3 directionToPoint = Vector3.Normalize(vector - _lookAtAnchor.position);
            Vector3 newPosition = vector + directionToPoint * addedLength;
            return newPosition;
        }
    }
}