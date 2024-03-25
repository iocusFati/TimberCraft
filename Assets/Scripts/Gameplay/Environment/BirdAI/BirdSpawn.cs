using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.WindData;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Environment.BirdAI
{
    public class BirdSpawn : SerializedMonoBehaviour
    {
        [SerializeField] private List<List<Bird>> _birdGroups;
        
        private EnvironmentConfig _environmentConfig;
        private CompositeDisposable _restoreCheckDisposer;
        private CompositeDisposable _restoreDisposer;

        private void Awake()
        {
            _restoreDisposer = new CompositeDisposable();
            _restoreCheckDisposer = new CompositeDisposable();
            
            SubscribeToRestoreCheckAll();
        }

        [Inject]
        public void Construct(IStaticDataService staticData)
        {
            _environmentConfig = staticData.EnvironmentConfig;
        }

        private void RestoreCheckRx(Bird bird, List<Bird> group) =>
            bird.HasFlownAway.Where(_ => AllFromGroupHaveFlown(group))
                .Subscribe(_ => RestoreBirdsInCooldownRx(group))
                .AddTo(_restoreCheckDisposer);

        private void RestoreBirdsInCooldownRx(List<Bird> birds)
        {
            IDisposable stream = Observable.Timer(GetRandomCooldown())
                .Subscribe(_ => RestoreBirds(birds))
                .AddTo(_restoreDisposer);
        }

        private void SubscribeToRestoreCheckAll()
        {
            foreach (var group in _birdGroups) 
                SubscribeToRestore(group);
        }

        private void SubscribeToRestore(List<Bird> group)
        {
            foreach (var bird in group) 
                RestoreCheckRx(bird, group);
        }

        private void RestoreBirds(List<Bird> birds)
        {
            foreach (var bird in birds) 
                bird.InitializeAsync().Forget();
            
            SubscribeToRestore(birds);
            
            _restoreDisposer.Clear();
        }

        private static bool AllFromGroupHaveFlown(List<Bird> group) => 
            group.All(bird => bird.HasFlownAway.Value);

        private TimeSpan GetRandomCooldown() => 
            TimeSpan.FromSeconds(Random.Range(
                _environmentConfig.BirdAppearCooldown.Item1,
                _environmentConfig.BirdAppearCooldown.Item2));

        private void OnDestroy()
        {
            _restoreDisposer.Clear();
            _restoreCheckDisposer.Clear();
        }
    }
}