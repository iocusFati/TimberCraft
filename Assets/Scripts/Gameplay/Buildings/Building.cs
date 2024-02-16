using System.Collections.Generic;
using Gameplay.Resource;
using Infrastructure.Services.SaveLoad;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UI;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Buildings
{
    [RequireComponent(typeof(UniqueId))]
    public abstract class Building : SerializedMonoBehaviour, IResourceBuildingReceivable
    {
        [Header("Main building")]
        [SerializeField] private Transform _receiveResourceTransform;
        [SerializeField] private ResourceCounter _resourceCounter;
        [SerializeField] private ResourceType _constructionResourceType;
        
        [OdinSerialize] private List<(GameObject Stage, int ResourceQuantity)> _constructionStagesAndResources;

        private ISaveLoadService _saveLoadService;

        private BuildingConstruction BuildingConstruction { get; set; }
        public Transform ReceiveResourceTransform => _receiveResourceTransform;
        public ResourceType ConstructionResourceType => _constructionResourceType;
        public bool IsBuilt => BuildingConstruction.IsBuilt;

        public abstract void InteractWithPlayer();
        public abstract void StopInteractingWithPlayer();

        [Inject]
        public void Construct(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        private void Start()
        {
            UniqueId id = GetComponent<UniqueId>();
            
            BuildingConstruction = new BuildingConstruction(id.Id, _constructionStagesAndResources, _resourceCounter);
            
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