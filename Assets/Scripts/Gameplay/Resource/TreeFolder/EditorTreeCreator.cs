using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Gameplay.Resource
{
    public class EditorTreeCreator : MonoBehaviour
    {
        [SerializeField] private int _resourcesCount;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _resourcePrefab;
        [SerializeField] private GameObject _treeTipPrefab;
        
        [Header("Distances")]
        [SerializeField] private float _distanceBetweenResources;
        [SerializeField] private float _distanceBetweenResourceAndTip;

        [Header("Parents")]
        [SerializeField] private Transform _resourceParent;
        [SerializeField] private Transform _treeTipParent;

        [Header("Materials")]
        [SerializeField] private Material _treeTipMaterial;


        [Button]
        public void SpawnTree()
        {
            ClearTree();

            Tree tree = GetComponent<Tree>();
            Type type = tree.GetType();
            FieldInfo segmentsFieldInfo = type.GetField("_segments", BindingFlags.NonPublic | BindingFlags.Instance);

            List<Transform> segments = (List<Transform>)segmentsFieldInfo.GetValue(tree);
            segments.Clear();

            for (int index = 0; index < _resourcesCount; index++)
            {
                GameObject resource = Instantiate(_resourcePrefab, _resourceParent);

                resource.transform.localPosition += new Vector3(0, _distanceBetweenResources * index, 0);
                
                segments.Add(resource.transform);
            }

            GameObject treeTip = Instantiate(_treeTipPrefab, _treeTipParent);
            
            _treeTipParent.transform.position = segments[^1].position + new Vector3(0, _distanceBetweenResourceAndTip);

            MeshRenderer treeTipMeshRenderer = treeTip.GetComponent<MeshRenderer>();
            treeTipMeshRenderer.material = _treeTipMaterial;
            
            FieldInfo meshRendererFieldInfo = 
                type.GetField("_treeTipMeshRenderer", BindingFlags.NonPublic | BindingFlags.Instance);
            meshRendererFieldInfo.SetValue(tree, treeTipMeshRenderer);
        }

        private void ClearTree()
        {
            _resourceParent.GetRidOfChildren();
            _treeTipParent.GetRidOfChildren();
        }
    }
}