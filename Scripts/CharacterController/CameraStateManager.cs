using System;
using System.Collections.Generic;
using Cinemachine;
using DoubTech.MCC.Input;
using DoubTech.Networking;
using UnityEngine;

namespace DoubTech.MCC.CharacterController
{
    public class CameraStateManager : MonoBehaviour
    {
        [Header("Virtual Cameras")]
        [SerializeField] private CinemachineVirtualCamera tpsCamera;
        [SerializeField] private CinemachineVirtualCamera tpsAimCamera;
        [SerializeField] private CinemachineVirtualCamera fpsCamera;
        [SerializeField] private CinemachineVirtualCamera fpsAimCamera;

        [Header("Input System")]
        [SerializeField] private InputSystemInput input;

        [Header("Camera State Properties")]
        [SerializeField] private float aimSensitivityMultiplier = .25f;
        

        [Header("Priorities")]
        [SerializeField] private int activePriority = 20;
        [SerializeField] private int inactivePriority = 10;

        [Header("State")]
        [SerializeField] private bool isFps;
        [SerializeField] private bool isAim;

        private List<CinemachineVirtualCamera> cameras;

        private float normalSensitivity;

        private void Awake()
        {
            normalSensitivity = input.sensitivity;
            cameras = new List<CinemachineVirtualCamera>()
            {
                tpsCamera, tpsAimCamera, fpsCamera, fpsAimCamera
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
            if (isAim)
            {
                input.sensitivity = normalSensitivity * aimSensitivityMultiplier;
                ActivateCamera(isFps ? fpsAimCamera : tpsAimCamera);
            }
            else
            {
                input.sensitivity = normalSensitivity;
                ActivateCamera(isFps ? fpsCamera : tpsCamera);
            }
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
