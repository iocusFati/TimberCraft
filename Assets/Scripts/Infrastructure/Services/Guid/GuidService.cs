using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Infrastructure.Services.Guid
{
    public class GuidService : SerializedMonoBehaviour, IGuidService
    {
        [OdinSerialize] private Dictionary<List<GameObject>, string> _guids;

        public string GetGuidFor(GameObject obj)
        {
            return ListIsContainingObj(obj, out List<GameObject> keyList) 
                ? _guids[keyList] 
                : string.Empty;
        }

        public List<GameObject> GetGameObjectFor(string id)
        {
            return _guids.FirstOrDefault(guidPair => guidPair.Value == id).Key;
        }

        private bool ListIsContainingObj(GameObject obj, out List<GameObject> keyList)
        {
            keyList = _guids.Keys.FirstOrDefault(list => list.Contains(obj));
            
            return keyList is not null;
        }

        [Button]
        private void GenerateGuids()
        {
            var nullGuidKeys = _guids.Keys
                .Where(key => _guids[key] == string.Empty || _guids[key] is null)
                .ToList();
            
            foreach (var nullGuidKey in nullGuidKeys)
            {
                _guids[nullGuidKey] = GenerateId();
            }
        }

        private string GenerateId() => 
            System.Guid.NewGuid().ToString();
    }
}