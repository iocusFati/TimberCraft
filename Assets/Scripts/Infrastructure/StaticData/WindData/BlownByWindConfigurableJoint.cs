using System.Collections;
using System.Threading;
using Gameplay.Wind;
using Infrastructure.Services.StaticDataService;
using UnityEngine;
using Zenject;

namespace Infrastructure.StaticData.WindData
{
    [RequireComponent(typeof(ConfigurableJoint))]
    public class BlownByWindConfigurableJoint : MonoBehaviour, IWindEffectable
    {
        [SerializeField] private float _turbulenceScale;
        [SerializeField] private float _turbulenceIntensity;
        [SerializeField] private float _startReducingForceTime;
        [SerializeField] private float _hitHeight = 1.5f;

        private WindSimulationConfig _windConfig;

        private float _windDuration;

        private ConfigurableJoint _joint;
        private Rigidbody _rb;

        private CancellationTokenSource _tokenSource;

        [Inject]
        public void Construct(IStaticDataService staticData)
        {
            _windConfig = staticData.WindConfig;
            _windDuration = _windConfig.CycleCount * _windConfig.RepeatInterval;
        }
        
        private void Awake()
        {
            _joint = GetComponent<ConfigurableJoint>();
            _rb = GetComponent<Rigidbody>();
            
            _joint.configuredInWorldSpace = true;
        }

        public void GetBlownWith(Force force)
        {
            StartCoroutine(ApplyForce(force));
        }

        private IEnumerator ApplyForce(Force force)
        {
            float time = 0;
            float windForce;

            float initialWindForce = force.Magnitude;

            while (time < _windDuration - _startReducingForceTime)
            {
                ApplyTurbulence();
                AddForce();
                
                time += Time.fixedDeltaTime;

                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            void ApplyTurbulence()
            {
                float forceTurbulence = (Mathf.PerlinNoise(Time.time * _turbulenceScale, 1000) * 2 - 1) * _turbulenceIntensity;
                windForce = initialWindForce + forceTurbulence;
            }
            
            void AddForce() =>
                _rb.AddForceAtPosition(force.Direction * windForce,
                    transform.position + new Vector3(0, _hitHeight, 0));
        }
        
        // private void OnDrawGizmos()
        // {
        //     Gizmos.DrawRay(transform.position + new Vector3(0, _hitHeight, 0), _forceDirection);
        // }

        public void OnWindStopped()
        {
            
        }
    }
}