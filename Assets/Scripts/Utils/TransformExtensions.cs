using UnityEngine;

namespace Utils
{
    public static class TransformExtensions
    {
        public static Transform FindParentWithTag(this Transform transform, string tag)
        {
            Transform currentChild = transform;
            
            while (!currentChild.parent.CompareTag(tag) || currentChild.parent is null) 
                currentChild = currentChild.parent;

            return currentChild.parent;
        }

        public static void GetRidOfChildren(this Transform transform)
        {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                Object.DestroyImmediate(child.gameObject);
                Debug.Log("DESTROY");
            }
        }
    }
}