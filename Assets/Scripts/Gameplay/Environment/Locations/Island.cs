using System.Collections.Generic;
using Gameplay.Environment.Buildings;
using Infrastructure.Services.SaveLoad;
using Pathfinding;
using UnityEngine;
using Zenject;

namespace Gameplay.Environment.Locations
{
    public class Island : MonoBehaviour
    {
        [SerializeField] private int _resourcesValue;
        [SerializeField] private ResourceSourcesHolder _resourceSourcesHolder;
        
        [SerializeField] private List<MinionHut> _minionHuts;

        [Header("Navigation")]
        [SerializeField] private GraphUpdateScene _graphUpdateScene;
        
        private ISaveLoadService _saveLoad;

        public List<MinionHut> MinionHuts => _minionHuts;
        public ResourceSourcesHolder ResourceSourcesHolder => _resourceSourcesHolder;

        [Inject]
        public void Construct(ISaveLoadService saveLoad)
        {
            _saveLoad = saveLoad;
        }
        
        private void OnEnable()
        {
            if (_graphUpdateScene != null) 
                AstarPath.active.UpdateGraphs(_graphUpdateScene.GetGraphUpdate());
            
            
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
                minionHut.Construct(_resourceSourcesHolder);
        }
    }
}