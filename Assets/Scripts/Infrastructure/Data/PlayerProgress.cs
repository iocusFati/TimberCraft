using System;
using System.Collections.Generic;

namespace Infrastructure.Data
{
    public class PlayerProgress
    {
        public Dictionary<string, BuildingSaveData> BuildingsSaveData = new();
        
        public bool WasLoaded { get; set; }
    }

    [Serializable]
    public class BuildingSaveData
    {
        public int CurrentlyConstructionNeededResourceNumber;
    }
}