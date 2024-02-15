using System;
using System.Collections.Generic;
using Gameplay.Resource;

namespace Infrastructure.Data
{
    public class PlayerProgress
    {
        public Dictionary<string, BuildingSaveData> BuildingsSaveData = new();
        public Dictionary<ResourceType, int> CollectedResources = new();

        public bool WasLoaded { get; set; }

        public void TryAddBuildingSaveDataPair(string id, BuildingSaveData buildingSaveData)
        {
            if (!BuildingsSaveData.ContainsKey(id)) 
                BuildingsSaveData.Add(id, new BuildingSaveData());
        }

        public BuildingSaveData GetBuildingSaveData(string guid) => 
            !BuildingsSaveData.ContainsKey(guid) 
                ? null 
                : BuildingsSaveData[guid];
    }

    [Serializable]
    public class BuildingSaveData
    {
        public int CurrentlyConstructionNeededResourceNumber;
        public int BuildingLevel;
        
        public bool IsBuilt => 
            CurrentlyConstructionNeededResourceNumber <= 0;
    }
}