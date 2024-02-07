using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.States
{
    public class LumberjackStorage
    {
        private readonly int _storageCapacity;

        private int _freePositionIndex;
        
        private readonly Stack<Transform> _resourceDropouts = new();
        private readonly Vector3[] _resourcePositions;
        public bool IsFull => _resourceDropouts.Count >= _storageCapacity;

        public LumberjackStorage(int storageCapacity)
        {
            _storageCapacity = storageCapacity;

            _resourcePositions = new Vector3[storageCapacity];
        }

        public Vector3 GetFreePosition() => 
            _resourcePositions[_freePositionIndex];

        public void OccupyFreePosition()
        {
            if (_freePositionIndex < _storageCapacity - 1) 
                _freePositionIndex++;
            else
                Debug.LogError("There is no free position left");
        }
    }
}