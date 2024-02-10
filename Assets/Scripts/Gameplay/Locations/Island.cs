using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Locations
{
    public class Island : MonoBehaviour
    {
        [SerializeField] private List<MinionHut> _minionHuts;
        [SerializeField] private ResourceSourcesHolder _resourceSourcesHolder;

        public List<MinionHut> MinionHuts => _minionHuts;
        public ResourceSourcesHolder ResourceSourcesHolder => _resourceSourcesHolder;

        public void Initialize()
        {
            foreach (var minionHut in _minionHuts)
            {
                minionHut.SpawnBots(_resourceSourcesHolder);
            }
        }
    }
}