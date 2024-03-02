using System.Collections.Generic;
using System.Linq;
using Gameplay.Buildings;
using Infrastructure.Data;
using Infrastructure.Services.Guid;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zenject;

namespace Gameplay.GameFlow
{
    public class BuildingDiscover : SerializedMonoBehaviour, ISavedProgress
    {
        [OdinSerialize] private Dictionary<Building, Building> _buildings;
        
        private HashSet<Building> _unlockedBuildings = new();
        private IGuidService _guidService;

        private List<string> UnlockedBuildingsIds => 
            _unlockedBuildings
                .Select(building => _guidService.GetGuidFor(building.gameObject))
                .ToList();

        [Inject]
        public void Construct(ISaveLoadService saveLoad, IGuidService guidService)
        {
            _guidService = guidService;
            
            saveLoad.Register(this);
        }

        public void UnlockWithKey(Building buildingKey)
        {
            if (_buildings.ContainsKey(buildingKey))
            {
                buildingKey.Unlock();
                _unlockedBuildings.Add(buildingKey);
                
                SubscribeToOnBuilt(buildingKey);
            }
        }

        public void OnProgressCouldNotBeLoaded()
        {
            HideAllLocked();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            List<IEnumerable<Building>> buildingsOutOfProgress = GetBuildingsOutOfProgress(progress).ToList();

            List<Building> unlockedBuildingsList = buildingsOutOfProgress
                .SelectMany(list => list)
                .ToList();
            
            _unlockedBuildings = new HashSet<Building>(unlockedBuildingsList);
            
            SubscribeToOnBuilt(unlockedBuildingsList[^1]);

            HideAllLocked();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.UnlockedBuildingsIds = UnlockedBuildingsIds;
        }

        private IEnumerable<IEnumerable<Building>> GetBuildingsOutOfProgress(PlayerProgress progress)
        {
            return progress.UnlockedBuildingsIds
                .Select(id => _guidService.GetGameObjectFor(id)
                    .Select(building => building.GetComponent<Building>())
                    .Where(building => building is not null));
        }

        private void HideAllLocked()
        {
            List<Building> enumerable = _buildings.Keys.Except(_unlockedBuildings).ToList();
            foreach (var building in enumerable) 
                building.gameObject.SetActive(false);
        }

        private void SubscribeToOnBuilt(Building buildingKey)
        {
            buildingKey.OnBuiltEvent += () =>
            {
                if (_buildings[buildingKey] is not null) 
                    UnlockWithKey(_buildings[buildingKey]);
            };
        }
    }
}