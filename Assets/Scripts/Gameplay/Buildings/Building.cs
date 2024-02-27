using System.Collections.Generic;
using Gameplay.Resource;
using Infrastructure.Services.Guid;
using Infrastructure.Services.SaveLoad;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UI;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Buildings
{
    public abstract class Building : SerializedMonoBehaviour, IResourceBuildingReceivable
    {
        [Header("Main building")] 
        
        [SerializeField] private Transform _receiveResourceTransform;
        [SerializeField] private bool _useServiceGuid = true;
        [SerializeField] private bool _resourceCounterNeeded;
        
        [ShowIf("_resourceCounterNeeded")]
        [SerializeField] private ResourceCounter _resourceCounter;
        [SerializeField] private ResourceType _constructionResourceType;
        [OdinSerialize] private List<(GameObject Stage, int ResourceQuantity)> _constructionStagesAndResources;

        protected ISaveLoadService _saveLoadService;

        private BuildingConstruction BuildingConstruction { get; set; }
        public Transform ReceiveResourceTransform => _receiveResourceTransform;
        public ResourceType ConstructionResourceType => _constructionResourceType;
        
        public bool IsBuilt => BuildingConstruction.IsBuilt;
        public int NeededResources => BuildingConstruction.CurrentlyNeededResourceQuantity;

        public abstract void InteractWithPlayer();
        public abstract void StopInteractingWithPlayer();

        [Inject]
        public void Construct(ISaveLoadService saveLoadService, IGuidService guidService)
        {
            _saveLoadService = saveLoadService;
            
            string id = GetID(guidService);
            
            BuildingConstruction = new BuildingConstruction(id, _constructionStagesAndResources, _resourceCounter);
        }

        private string GetID(IGuidService guidService)
        {
            if (_useServiceGuid)
            {
                return guidService.GetGuidFor(gameObject);
            }
            else
            {
                return GetComponent<UniqueId>().Id;
            }
        }

        private void Start()
        {
            _saveLoadService.Register(BuildingConstruction);
        }

        public void ReceiveResource(int resourceQuantity)
        {
            if (!BuildingConstruction.IsBuilt)
                BuildingConstruction.BuildWith(resourceQuantity, OnBuilt);
        }

        [Button]
        public void BuildInstantly()
        {
            BuildingConstruction.BuildWith(100000, OnBuilt);
        }

        protected virtual void OnBuilt()
        {
            
        }
    }
}