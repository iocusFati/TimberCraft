using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Pool;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.WindData;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

namespace Gameplay.Environment.Wind
{
    public class WindSimulator : MonoBehaviour 
    {
        [SerializeField] private ParticleSystem _windFX;
        [SerializeField] private Transform _windZoneCenter;
        [SerializeField] private Transform _activeZone;
        [SerializeField] private List<GameObject> _blowObjects;

        private LayerMask PlayerLayer;
        
        private ParticlePool _windParticlePool;
        private EnvironmentConfig _environmentConfig;
        private CompositeDisposable _disposer;

        [Inject]
        public void Construct(IStaticDataService staticData)
        {
            _environmentConfig = staticData.EnvironmentConfig;
        }

        private void Awake()
        {
            _disposer = new CompositeDisposable();
            PlayerLayer = LayerMask.GetMask("PlayerBody");
        }

        private void OnEnable() => 
            BlowWindInTime();

        private void OnDisable() => 
            _disposer.Clear();

        private void BlowWindInTime()
        {
            IDisposable blowStream = Observable.Timer(GetRandomCooldown())
                .Where(_ => CheckForPlayer())
                .Subscribe(_ => SimulateWind())
                .AddTo(_disposer);
        }

        private bool CheckForPlayer()
        {
            Collider[] results = new Collider[1];
            Physics.OverlapBoxNonAlloc(_activeZone.position, _activeZone.lossyScale * 0.5f, results,
                _activeZone.rotation, PlayerLayer);

            if (results[0] is not null)
            {
                return true;
            }

            _disposer.Clear();
            BlowWindInTime();

            return false;
        }

        private TimeSpan GetRandomCooldown() => 
            TimeSpan.FromSeconds(Random.Range(_environmentConfig.MinCooldown, _environmentConfig.MaxCooldown));
        
        [Button]
        public void TestSimulate()
        {
            SimulateWind();
        }
        
        public void SimulateWind()
        {
            Force force = GetRandomForce();
            
            SetWindFX(_windZoneCenter, force);
            Blow(force);
            
            _disposer.Clear();
        }

        private void Blow(Force force)
        {
            foreach (var blown in _blowObjects)
            {
                if (blown.TryGetComponent(out IWindEffectable windEffectable))
                {
                    windEffectable.GetBlownWith(force);

                    StopBlowingAsync(windEffectable).Forget();
                }
            }
        }

        private async UniTaskVoid StopBlowingAsync(IWindEffectable windEffectable)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_environmentConfig.Duration));

            windEffectable.OnWindStopped();
            BlowWindInTime();
        }

        private Force GetRandomForce()
        {
            float randomAngle = Random.Range(0, 90);

            Vector3 direction = GetDirection(randomAngle);
            float magnitude = Random.Range(_environmentConfig.MinForceMagnitude, _environmentConfig.MaxForceMagnitude);

            Force force = new Force
            {
                Angle = randomAngle,
                Direction = direction,
                Magnitude = magnitude
            };

            return force;
        }

        private void SetWindFX(Transform box, Force force)
        {
            EmissionModule windFX_emission = _windFX.emission;
            
            Burst burst = new Burst(0, _environmentConfig.BurstCount, _environmentConfig.CycleCount, _environmentConfig.RepeatInterval);
            windFX_emission.SetBurst(0, burst);
            
            _windFX.transform.position = box.position;
            _windFX.transform.rotation = Quaternion.Euler(new Vector3(0, 90 - force.Angle, 0));
            
            _windFX.Play();
        }

        private static Vector3 GetDirection(float randomAngle)
        {
            float randomAngleRad = randomAngle * Mathf.Deg2Rad;
            float xDir = Mathf.Cos(randomAngleRad);
            float zDir = Mathf.Sin(randomAngleRad);
            
            Vector3 direction = new Vector3(xDir, 0, zDir).normalized;
            
            return direction;
        }
    }

    public class Force
    {
        public Vector3 Direction { get; set; }
        public float Magnitude { get; set; }
        public float Angle { get; set; }
    }
}