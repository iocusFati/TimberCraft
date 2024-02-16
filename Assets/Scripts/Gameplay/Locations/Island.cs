using System.Collections.Generic;
using Gameplay.Buildings;
using Infrastructure.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace Gameplay.Locations
{
    public class Island : MonoBehaviour
    {
        [SerializeField] private int _resourcesValue;
        [SerializeField] private ResourceSourcesHolder _resourceSourcesHolder;
        
        [SerializeField] private List<MinionHut> _minionHuts;
        
        private ISaveLoadService _saveLoad;

        public List<MinionHut> MinionHuts => _minionHuts;
        public ResourceSourcesHolder ResourceSourcesHolder => _resourceSourcesHolder;

        [Inject]
        public void Construct(ISaveLoadService saveLoad)
        {
            _saveLoad = saveLoad;
        }

        public void Initialize()
        {
            InitializeMinionHuts();

            foreach (var resourceSource in ResourceSourcesHolder.AllResourceSources)
            {
                resourceSource.Construct(_resourcesValue);
            }
        }

        private void InitializeMinionHuts()
        {
            foreach (var minionHut in _minionHuts)
            {
                minionHut.Construct(_resourceSourcesHolder);
                
                _saveLoad.Register(minionHut);
            }
        }
    }
}