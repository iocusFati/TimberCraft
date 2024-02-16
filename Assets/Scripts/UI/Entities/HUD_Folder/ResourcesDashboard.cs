using Gameplay.Resource;
using Gameplay.Resource.ResourceStorage;
using Infrastructure.Data;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace UI.Entities.HUD_Folder
{
    public class ResourcesDashboard : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private ResourceCounter _coinResourceCounter;
        [SerializeField] private ResourceCounter _woodResourceCounter;
        [SerializeField] private ResourceCounter _stoneResourceCounter;
        [SerializeField] private ResourceCounter _goldResourceCounter;
        
        private IGameResourceStorage _gameResourceStorage;

        [Inject]
        public void Construct(IGameResourceStorage gameResourceStorage, ISaveLoadService saveLoad)
        {
            _gameResourceStorage = gameResourceStorage;
            
            saveLoad.Register(this);
        }

        private void Start()
        {
            _gameResourceStorage.OnWoodCountChanged +=
                newCount => _woodResourceCounter.SetCountWith(newCount, true, true);
            _gameResourceStorage.OnStoneCountChanged +=
                newCount => _stoneResourceCounter.SetCountWith(newCount, true, true);
            _gameResourceStorage.OnCoinCountChanged +=
                newCount => _coinResourceCounter.SetCountWith(newCount, true, true);
            _gameResourceStorage.OnGoldCountChanged +=
                newCount => _goldResourceCounter.SetCountWith(newCount, true, true);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            int coinCount = progress.CollectedResources[ResourceType.Coin];
            int woodCount = progress.CollectedResources[ResourceType.Wood];
            int stoneCount = progress.CollectedResources[ResourceType.Stone];
            int goldCount = progress.CollectedResources[ResourceType.Gold];

            SetCounters(coinCount, woodCount, stoneCount, goldCount);
        }

        public void OnProgressCouldNotBeLoaded() => 
            SetCounters(0, 0, 0, 0);

        private void SetCounters(int coinCount, int woodCount, int stoneCount, int goldCount)
        {
            _coinResourceCounter.SetCountWith(coinCount, false, true);
            _woodResourceCounter.SetCountWith(woodCount, false, true);
            _stoneResourceCounter.SetCountWith(stoneCount, false, true);
            _goldResourceCounter.SetCountWith(goldCount, false, true);
        }
    }
}