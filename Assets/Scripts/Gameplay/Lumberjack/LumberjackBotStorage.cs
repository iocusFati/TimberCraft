
using System;
using System.Collections.Generic;
using Gameplay.Resource;
using Infrastructure.StaticData.LumberjackData;
using UnityEngine;

namespace Gameplay.Lumberjack
{
    public class LumberjackBotStorage
    {
        public readonly Stack<DropoutResource> ResourceDropouts = new();

        private readonly float _lootPositionOffset;
        private readonly float _storageFloors;

        private int FreePositionIndex => ResourceDropouts.Count;

        private readonly Transform _lootStackBottom;
        private readonly Transform[] _resourceCells;

        public int StorageCapacity { get; set; }
        public bool IsFull => ResourceDropouts.Count >= StorageCapacity;
        
        public event Action OnStorageFull;

        public LumberjackBotStorage(LumberjackBotConfig lumberjackBotConfig, Transform lootStackBottom)
        {
            _lootStackBottom = lootStackBottom;
            
            StorageCapacity = lumberjackBotConfig.InitialStorageCapacity;
            _lootPositionOffset = lumberjackBotConfig.LootPositionOffset;
            _storageFloors = lumberjackBotConfig.StorageFloors;

            _resourceCells = new Transform[StorageCapacity];
        }

        public void Initialize()
        {
            SetLootStackPositions();
        }

        public Transform GetFreeCell() => 
            _resourceCells[FreePositionIndex];

        public void OccupyFreePosition(DropoutResource dropout)
        {
            if (FreePositionIndex < StorageCapacity)
            {
                ResourceDropouts.Push(dropout);

                if (IsFull) 
                    OnStorageFull?.Invoke();
            }
            else
            {
                Debug.LogError("There is no free position left");
            }
        }

        public void LeaveResources()
        {
            foreach (var resource in ResourceDropouts) 
                resource.transform.SetParent(null);
        }

        private void SetLootStackPositions()
        {
            for (int index = 0; index < StorageCapacity; index++)
            {
                GetOffsets(index, out float offsetY, out float offsetZ);

                Transform resourceCell = CreateResourceCell(offsetY, offsetZ);

                _resourceCells[index] = resourceCell;
            }
        }

        private Transform CreateResourceCell(float offsetY, float offsetZ)
        {
            Transform resourceCell = new GameObject("ResourceCell").transform;
            
            resourceCell.SetParent(_lootStackBottom);
            resourceCell.localPosition = new Vector3(0, offsetY, offsetZ);
            resourceCell.localScale = Vector3.one;
            
            return resourceCell;
        }

        private void GetOffsets(int index, out float offsetY, out float offsetZ)
        {
            if (index == 0)
            {
                offsetY = offsetZ = 0;
                
                return;
            }
            
            // used to find out if index exceeds floors limit(_storageFloors) 
            float storageFloorModifier = (float)Math.Floor(index / _storageFloors);
            
            offsetY = (index - _storageFloors * storageFloorModifier) * _lootPositionOffset;
            offsetZ = storageFloorModifier * -_lootPositionOffset;
        }
    }
}