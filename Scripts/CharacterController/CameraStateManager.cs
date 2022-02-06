using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace DoubTech.MCC.CharacterController
{
    public class CameraStateManager : MonoBehaviour
    {
        [Header("Virtual Cameras")]
        [SerializeField] private CinemachineVirtualCamera tpsCamera;
        [SerializeField] private CinemachineVirtualCamera tpsAimCamera;
        [SerializeField] private CinemachineVirtualCamera fpsCamera;

        [Header("Priorities")]
        [SerializeField] private int activePriority = 20;
        [SerializeField] private int inactivePriority = 10;

        [Header("State")]
        [SerializeField] private bool isFps;
        [SerializeField] private bool isAim;

        private List<CinemachineVirtualCamera> cameras;

        private void Awake()
        {
            cameras = new List<CinemachineVirtualCamera>()
            {
                tpsCamera, tpsAimCamera, fpsCamera
            };
        }

        public bool IsFps
        {
            get => isFps;
            set
            {
                isFps = value;
                UpdateCamera();
            }
        }

        public bool IsAiming
        {
            get => isAim;
            set
            {
                isAim = value;
                UpdateCamera();
            }
        }

        public void ToggleFPS()
        {
            IsFps = !IsFps;
        }

        private void UpdateCamera()
        {
            if(!isAim) ActivateCamera(isFps ? fpsCamera : tpsCamera);
            else ActivateCamera(isFps ? fpsCamera : tpsAimCamera);
        }

        public void ActivateCamera(CinemachineVirtualCamera camera)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                if (cameras[i] != camera) cameras[i].Priority = inactivePriority;
            }
            camera.Priority = activePriority;
        }
    }
}
