using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Animations
{
    public class ParabolicTrajectory : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _curve;
        
        [SerializeField] private float _duration;
        [SerializeField] private float _maxHeightY;
        [SerializeField] private Transform _target;

        [Button]
        public void Move()
        {
            StartCoroutine(Curve(transform.position, _target.position));
        }

        private IEnumerator Curve(Vector3 start, Vector3 finish)
        {
            var timePast = 0f;
            
            while (timePast < _duration)
            {
                timePast += Time.deltaTime;
                Debug.Log(timePast);

                float linearTime = timePast / _duration; //0 to 1 time
                float heightTime = _curve.Evaluate(linearTime); //value from curve

                var height = Mathf.Lerp(0f, _maxHeightY, heightTime); //clamped between the max height and 0

                transform.position =
                    Vector3.Lerp(start, finish, linearTime) + new Vector3(0f, height, 0f); //adding values on y axis

                yield return null;
            }
        }
    }
}