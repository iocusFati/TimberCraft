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
    }
}