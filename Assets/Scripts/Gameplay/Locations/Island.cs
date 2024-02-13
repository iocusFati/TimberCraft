using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Locations
{
    public class Island : MonoBehaviour
    {
        [SerializeField] private int _resourcesValue;
        [SerializeField] private ResourceSourcesHolder _resourceSourcesHolder;
        
        [SerializeField] private List<MinionHut> _minionHuts;

        public List<MinionHut> MinionHuts => _minionHuts;
        public ResourceSourcesHolder ResourceSourcesHolder => _resourceSourcesHolder;

        public void Initialize()
        {
            SpawnBots();

            foreach (var resourceSource in ResourceSourcesHolder.AllResourceSources)
            {
                resourceSource.Construct(_resourcesValue);
            }
        }

        private void SpawnBots()
        {
            foreach (var minionHut in _minionHuts) 
                minionHut.SpawnBots(_resourceSourcesHolder);
        }
    }
}