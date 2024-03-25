using System.Collections.Generic;
using System.Linq;
using Gameplay.Resource;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Environment.Locations
{
    public class ResourceSourcesHolder : SerializedMonoBehaviour
    {
        public Dictionary<ResourceType, List<ResourceSource>> ResourceSources;
        public List<ResourceSource> AllResourceSources => 
            ResourceSources.Values
                .SelectMany(list => list)
                .ToList(); 

        public ResourceSource GetClosestSourceOfType(ResourceType type, Vector3 lumberjackPosition,
            ResourceSourceState resourceSourceState = ResourceSourceState.Untouched)
        {
            return ResourceSources[type]
                .Where(resourceSource => resourceSource.CurrentState == resourceSourceState)
                .OrderBy(resourceSource => (resourceSource.transform.position - lumberjackPosition).magnitude)
                .FirstOrDefault();
        }
    }
}