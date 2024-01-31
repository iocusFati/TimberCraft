using System.Collections;
using Infrastructure.Services.Pool;
using Infrastructure.Services.StaticDataService;
using UnityEngine;

namespace Infrastructure.States
{
    public abstract class ResourceSource : MonoBehaviour
    {
        protected abstract float RestoreSourceAfter { get; set; }
        
        public virtual void GetDamage(Vector3 hitPoint, Transform hitTransform)
        {
            
        }

        protected virtual void RestoreSource()
        {
            
        }

        protected IEnumerator WaitAndRestoreSource()
        {
            yield return new WaitForSeconds(RestoreSourceAfter);
            
            RestoreSource();
        }
    }
}