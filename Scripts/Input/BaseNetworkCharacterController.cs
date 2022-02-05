﻿using UnityEngine;

namespace DoubTech.MCC.Input
{
    public abstract class BaseNetworkCharacterController : MonoBehaviour
    {
        [SerializeField] private NetworkInputSync playerInput;

        [Header("Camera Movement")]
        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip(
            "Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        [Header("Cinemachine")]
        [Tooltip(
            "The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject FPSCameraTarget;
        public GameObject TPSCameraTarget;
        
        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        private const float _threshold = 0.01f;
        
        protected IPlayerInputSync playerInputSync;

        protected virtual void OnEnable()
        {
            playerInputSync = GetComponentInParent<IPlayerInputSync>();
        }
        
        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (playerInputSync.Look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                _cinemachineTargetYaw += playerInputSync.Look.x * Time.deltaTime;
                _cinemachineTargetPitch += playerInputSync.Look.y * Time.deltaTime;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw =
                ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            FPSCameraTarget.transform.rotation = TPSCameraTarget.transform.rotation = Quaternion.Euler(
                _cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        protected virtual void LateUpdate()
        {
            CameraRotation();
        }
    }
}