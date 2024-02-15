using Pathfinding.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class UniqueId : MonoBehaviour
    {
        public string Id;

        [Button]
        public void GenerateId() =>
            Id = $"_{Guid.NewGuid().ToString()}";
    }
}