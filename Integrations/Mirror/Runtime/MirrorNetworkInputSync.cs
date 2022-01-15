#if MIRROR
using Mirror;
using UnityEngine;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace DoubTech.Multiplayer.Input
{
    [RequireComponent(typeof(NetworkInputSync))]
    public class MirrorNetworkInputSync : NetworkBehaviour, IPlayerInputSync
    {
        private NetworkInputSync networkInputSync;

        private void Awake()
        {
            networkInputSync = GetComponent<NetworkInputSync>();
        }

        [Command]
        public void UpdateCameraAngleServerRpc(float cameraAngle)
        {
            networkInputSync.cameraAngle = cameraAngle;
        }

        [Command]
        public void UpdateJumpServerRpc(bool inputJump)
        {
            networkInputSync.inputJump = inputJump;
            networkInputSync.onJump?.Invoke();
        }

        [Command]
        public void UpdateAnalogMoveServerRpc(bool inputAnalogMovement)
        {
            networkInputSync.inputAnalogMovement = inputAnalogMovement;
        }

        [Command]
        public void UpdateMoveServerRpc(Vector2 inputMove)
        {
            networkInputSync.inputMove = inputMove;
        }

        [Command]
        public void UpdateSprintServerRpc(bool inputSprint)
        {
            networkInputSync.inputSprint = inputSprint;
        }

        [Command]
        public void UpdateLookServerRpc(Vector2 inputLook)
        {
            networkInputSync.inputLook = inputLook;
        }

        public void UpdateCameraAngle(float cameraAngle)
        {
            UpdateCameraAngleServerRpc(cameraAngle);
        }

        public void UpdateJump(bool inputJump)
        {
            UpdateJumpServerRpc(inputJump);
        }

        public void UpdateAnalogMove(bool inputAnalogMovement)
        {
            UpdateAnalogMoveServerRpc(inputAnalogMovement);
        }

        public void UpdateMove(Vector2 inputMove)
        {
            UpdateMoveServerRpc(inputMove);
        }

        public void UpdateSprint(bool inputSprint)
        {
            UpdateSprintServerRpc(inputSprint);
        }

        public void UpdateLook(Vector2 inputLook)
        {
            UpdateLookServerRpc(inputLook);
        }
    }
}
#endif