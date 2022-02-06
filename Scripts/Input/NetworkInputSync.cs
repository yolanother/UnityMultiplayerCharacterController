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
        [SerializeField] private NetworkMultiplayerInput _input;
        [SerializeField] private Camera _mainCamera;

        private IPlayerInfoProvider playerInfo;
        private IPlayerInputSync inputSync;
        private double TOLERANCE = .0001f;

        private void OnEnable()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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