using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace TotallyNotEvil
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cam;

        private void Start()
        {
            cam = GetComponent<CinemachineVirtualCamera>();
        }


        /// <summary>
        /// Sets the target to look at.
        /// </summary>
        /// <param name="trans">The new transform.</param>
        public void SetTargetAndFollow(Transform trans)
        {
            cam.Follow = trans;
            cam.LookAt = trans;
        }
    }
}
