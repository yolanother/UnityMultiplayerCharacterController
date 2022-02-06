using UnityEngine;
using UnityEngine.InputSystem;

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
        [SerializeField] private float _cinemachineTargetYaw;
        [SerializeField] private float _cinemachineTargetPitch;

        [SerializeField] private float inputThreshold = 0.001f;
        [SerializeField] private float sensitivity = 1;
        [SerializeField] private bool invertY = false;
        [SerializeField] private bool invertX = false;
        
        protected IPlayerInputSync playerInputSync;

        protected virtual void OnEnable()
        {
            playerInputSync = GetComponentInParent<IPlayerInputSync>();
        }
        
        private void CameraRotation()
        {
            if (null == playerInputSync) return;
            
            // if there is an input and camera position is not fixed
            if (playerInputSync.Look.sqrMagnitude >= inputThreshold && !LockCameraPosition)
            {
                _cinemachineTargetYaw += playerInputSync.Look.x * Time.deltaTime * sensitivity * (invertX ? -1 : 1);
                _cinemachineTargetPitch += playerInputSync.Look.y * Time.deltaTime * sensitivity * (invertY ? 1 : -1);
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

        protected virtual void Update()
        {
            CameraRotation();
        }
    }
}