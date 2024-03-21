using System;
using System.Collections.Generic;
using System.Threading;
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

namespace Gameplay.Wind
{
    public class WindSimulator : MonoBehaviour 
    {
        [SerializeField] private ParticleSystem _windFX;
        [SerializeField] private Transform _windZoneCenter;
        [SerializeField] private Transform _activeZone;
        [SerializeField] private List<GameObject> _blowObjects;

        private LayerMask PlayerLayer;
        
        private ParticlePool _windParticlePool;
        private WindSimulationConfig _windConfig;
        private CompositeDisposable _disposer;

        [Inject]
        public void Construct(IStaticDataService staticData)
        {
            _windConfig = staticData.WindConfig;
        }

        private void Awake()
        {
            _disposer = new CompositeDisposable();
            PlayerLayer = LayerMask.GetMask("PlayerBody");
            
            BlowWindInTime();
        }

        private void BlowWindInTime()
        {
            IDisposable blowStream = Observable.Timer(GetRandomCooldown())
                .Where(_ => CheckForPlayer())
                .First()
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
            TimeSpan.FromSeconds(Random.Range(_windConfig.MinCooldown, _windConfig.MaxCooldown));

        // private async UniTaskVoid SimulateWhenReadyAsync()
        // {
        //     await UniTask.Delay(TimeSpan.FromSeconds(waitForWindTime)) 
        // }

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
            await UniTask.Delay(TimeSpan.FromSeconds(_windConfig.Duration));

            windEffectable.OnWindStopped();
            BlowWindInTime();
        }

        private Force GetRandomForce()
        {
            float randomAngle = Random.Range(0, 90);

            Vector3 direction = GetDirection(randomAngle);
            float magnitude = Random.Range(_windConfig.MinForceMagnitude, _windConfig.MaxForceMagnitude);

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
            
            Burst burst = new Burst(0, _windConfig.BurstCount, _windConfig.CycleCount, _windConfig.RepeatInterval);
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