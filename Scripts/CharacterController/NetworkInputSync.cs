﻿using DoubTech.Networking;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace DoubTech.Multiplayer.Input
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkInputSync : NetworkBehaviour
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

        private NetworkMultiplayerInput _input;

        public GameObject _mainCamera;

        private const float _threshold = 0.01f;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        [Header("Events")]
        public UnityEvent onJump = new UnityEvent();

        public UnityEvent onSprintStarted = new UnityEvent();
        public UnityEvent onSprintEnded = new UnityEvent();

        [Header("Current Input State")]
        [ReadOnly] public Vector2 inputLook;
        [ReadOnly] public bool inputSprint;
        [ReadOnly] public Vector2 inputMove;
        [ReadOnly] public bool inputAnalogMovement;
        [ReadOnly] public bool inputJump;
        [ReadOnly] public float cameraAngle;

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void OnEnable()
        {
            _input = FindObjectOfType<NetworkMultiplayerInput>();
        }

        private void Update()
        {
            if (!NetworkManager.singleton || isLocalPlayer)
            {
                if (inputLook != _input.look)
                {
                    inputLook = _input.look;
                    UpdateLookServerRpc(_input.look);
                }

                if (inputSprint != _input.sprint)
                {
                    inputSprint = _input.sprint;
                    UpdateSprintServerRpc(inputSprint);
                }

                if (inputJump != _input.jump)
                {
                    inputJump = _input.jump;
                    UpdateJumpServerRpc(inputJump);
                }

                if (inputMove != _input.move)
                {
                    inputMove = _input.move;
                    UpdateMoveServerRpc(inputMove);
                }

                if (inputAnalogMovement != _input.analogMovement)
                {
                    inputAnalogMovement = _input.analogMovement;
                    UpdateAnalogMoveServerRpc(inputAnalogMovement);
                }

                if (cameraAngle != _mainCamera.transform.eulerAngles.y)
                {
                    cameraAngle = _mainCamera.transform.eulerAngles.y;
                    UpdateCameraAngleServerRpc(cameraAngle);
                }
            }
        }

        [Command]
        private void UpdateCameraAngleServerRpc(float cameraAngle)
        {
            this.cameraAngle = cameraAngle;
        }

        [Command]
        private void UpdateJumpServerRpc(bool inputJump)
        {
            this.inputJump = inputJump;
            onJump.Invoke();
        }

        [Command]
        private void UpdateAnalogMoveServerRpc(bool inputAnalogMovement)
        {
            this.inputAnalogMovement = inputAnalogMovement;
        }

        [Command]
        private void UpdateMoveServerRpc(Vector2 inputMove)
        {
            this.inputMove = inputMove;
        }

        [Command]
        private void UpdateSprintServerRpc(bool inputSprint)
        {
            this.inputSprint = inputSprint;
            if(inputSprint) onSprintStarted.Invoke();
            else onSprintEnded.Invoke();
        }

        [Command]
        private void UpdateLookServerRpc(Vector2 inputLook)
        {
            this.inputLook = inputLook;
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (inputLook.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                _cinemachineTargetYaw += inputLook.x * Time.deltaTime;
                _cinemachineTargetPitch += inputLook.y * Time.deltaTime;
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
}
