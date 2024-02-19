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
        [SerializeField] private bool _isConstructedByDefault;
        
        [HideIf("_isConstructedByDefault")]
        [SerializeField] private Transform _receiveResourceTransform;
        [HideIf("_isConstructedByDefault")]
        [SerializeField] private ResourceCounter _resourceCounter;
        [HideIf("_isConstructedByDefault")]
        [SerializeField] private ResourceType _constructionResourceType;
        [HideIf("_isConstructedByDefault")]
        [OdinSerialize] private List<(GameObject Stage, int ResourceQuantity)> _constructionStagesAndResources;

        protected ISaveLoadService _saveLoadService;

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