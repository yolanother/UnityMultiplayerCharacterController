using System;
using DoubTech.Networking;
using UnityEngine;
using UnityEngine.Events;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace DoubTech.MCC.Input
{
    public class NetworkInputSync : MonoBehaviour
    {
        [Header("Cinemachine")]
        [Tooltip(
            "The follow target set in the Cinemachine Virtual Camera that the camera will follow in FPS mode")]
        public GameObject FPSCameraTarget;

        [Tooltip(
            "The follow target set in the Cinemachine Virtual Camera that the camera will follow in TPS Mode")]
        public GameObject TPSCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip(
            "Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        [SerializeField] private NetworkMultiplayerInput _input;

        public GameObject _mainCamera;

        private const float _threshold = 0.01f;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        [Header("Events")]
        public UnityEvent onJump = new UnityEvent();

        public UnityEvent onSprintStarted = new UnityEvent();
        public UnityEvent onSprintEnded = new UnityEvent();

        private IPlayerInfoProvider playerInfo;
        private IPlayerInputSync inputSync;
        private double TOLERANCE = .0001f;

        private void OnEnable()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            _input = FindObjectOfType<NetworkMultiplayerInput>();
            playerInfo = GetComponentInParent<IPlayerInfoProvider>();
            inputSync = GetComponentInParent<IPlayerInputSync>();
        }

        private void Update()
        {
            if (!playerInfo.IsLocalPlayer) return;
            if (null == inputSync) return;
            if (null == _input) return;
            
            if (inputSync.Look != _input.look)
            {
                inputSync.Look = _input.look;
            }

            if (inputSync.Sprint != _input.sprint)
            {
                inputSync.Sprint = _input.sprint;
            }

            if (inputSync.Jump != _input.jump)
            {
                inputSync.Jump = _input.jump;
            }

            if (inputSync.Move != _input.move)
            {
                inputSync.Move = _input.move;
            }

            if (inputSync.AnalogMovement != _input.analogMovement)
            {
                inputSync.AnalogMovement = _input.analogMovement;
            }
            inputSync.CameraAngle = _mainCamera.transform.eulerAngles.y;
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void CameraRotation()
        {
            if (null == inputSync) return;
            
            // if there is an input and camera position is not fixed
            if (inputSync.Look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                _cinemachineTargetYaw += inputSync.Look.x * Time.deltaTime;
                _cinemachineTargetPitch += inputSync.Look.y * Time.deltaTime;
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
    }

    public interface IPlayerInputSync
    {
        float CameraAngle { get; set; }
        bool Jump { get; set; }
        bool AnalogMovement { get; set; }
        Vector2 Move { get; set; }
        bool Sprint { get; set; }
        Vector2 Look { get; set; }
    }
}