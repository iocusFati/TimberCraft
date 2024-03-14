using System;
using System.Collections.Generic;
using System.Reflection;
using Infrastructure.StaticData.ResourcesData;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private Material _treeTipDitherMaterial;
        [SerializeField] private Material _treeTipMaterial;

        [SerializeField] private TreeCreateConfig _treeCreateConfig;

        [Button]
        public void SpawnTree()
        {
            ClearTree();

            Tree tree = GetComponent<Tree>();
            Type type = tree.GetType();
            FieldInfo segmentsFieldInfo = type.GetField("_segments", BindingFlags.NonPublic | BindingFlags.Instance);

            List<Transform> segments = (List<Transform>)segmentsFieldInfo.GetValue(tree);
            segments.Clear();

            SpawnSegments(segments);
            SpawnTreeTip(segments, type, tree);
        }

        private void SpawnSegments(List<Transform> segments)
        {
            for (int index = 0; index < _resourcesCount; index++)
            {
                GameObject resource = Instantiate(_resourcePrefab, _resourceParent);
                ConstantForce resourceForce = resource.GetComponent<ConstantForce>();
                Rigidbody resourceRB = resource.GetComponent<Rigidbody>();

                resourceForce.force = new Vector3(
                    0, -_treeCreateConfig.ForcePerSegment * (4 - _resourcesCount + index + 1), 0);
                
                resourceRB.mass = 
                    _treeCreateConfig.MaxMass - _treeCreateConfig.MassPerSegment * (4 - _resourcesCount + index + 1); 
                
                resource.transform.localPosition += new Vector3(0, _distanceBetweenResources * index, 0);
                
                segments.Add(resource.transform);
            }
        }

        private void SpawnTreeTip(List<Transform> segments, Type type, Tree tree)
        {
            GameObject treeTip = Instantiate(_treeTipPrefab, _treeTipParent);
            GameObject treeTipDither = Instantiate(_treeTipPrefab, _treeTipParent);
            MeshRenderer treeTipMeshRenderer = treeTip.GetComponent<MeshRenderer>();
            MeshRenderer treeTipDitherMeshRenderer = treeTipDither.GetComponent<MeshRenderer>();
            Rigidbody treeTipRB = treeTip.GetComponentInParent<Rigidbody>();

            _treeTipParent.transform.position = segments[^1].position + new Vector3(0, _distanceBetweenResourceAndTip);

            treeTipDither.SetActive(false);
            
            treeTipMeshRenderer.material = _treeTipMaterial;
            treeTipDitherMeshRenderer.material = _treeTipDitherMaterial;

            treeTipRB.mass = _treeCreateConfig.TreeTipMass;
            
            FieldInfo meshRendererFieldInfo = 
                type.GetField("_treeTipObscureMeshRenderer", BindingFlags.NonPublic | BindingFlags.Instance);
            meshRendererFieldInfo.SetValue(tree, treeTipDitherMeshRenderer);
            
            FieldInfo mainGOFieldInfo = 
                type.GetField("_mainGo", BindingFlags.NonPublic | BindingFlags.Instance);
            mainGOFieldInfo.SetValue(tree, treeTip);
        }

        private void ClearTree()
        {
            _resourceParent.GetRidOfChildren();
            _treeTipParent.GetRidOfChildren();
        }
    }
}