using Cinemachine;
using UnityEngine;

namespace UI
{
    public class BillboardEffect : MonoBehaviour
    {
        private Transform mainCamera;

        private void Start()
        {
            mainCamera = GameObject.FindGameObjectWithTag("PlayerCamera").transform;
        }

        private void Update()
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up);
        }
    }
}