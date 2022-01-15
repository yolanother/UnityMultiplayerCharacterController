#if PUN_2_0_OR_NEWER
using DoubTech.Multiplayer;
using Photon.Pun;
using UnityEngine;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace DoubTech.Multiplayer.Input
{
    [RequireComponent(typeof(NetworkInputSync))]
    public class PunNetworkInputSync : MonoBehaviourPun, IPlayerInputSync
    {
        private NetworkInputSync networkInputSync;

        private void Awake()
        {
            networkInputSync = GetComponent<NetworkInputSync>();
        }

        public void UpdateCameraAngle(float cameraAngle)
        {
            photonView.RPC("UpdateCameraAngleRPC", RpcTarget.All, cameraAngle);
        }

        [PunRPC]
        protected void UpdateCameraAngleRPC(float cameraAngle)
        {
            networkInputSync.cameraAngle = cameraAngle;
        }

        public void UpdateJump(bool inputJump)
        {
            photonView.RPC("UpdateJumpRPC", RpcTarget.All, inputJump);
        }

        [PunRPC]
        protected void UpdateJumpRPC(bool inputJump)
        {
            networkInputSync.inputJump = inputJump;
            networkInputSync.onJump?.Invoke();
        }

        public void UpdateAnalogMove(bool inputAnalogMovement)
        {
            photonView.RPC("UpdateAnalogMoveRPC", RpcTarget.AllBufferedViaServer, inputAnalogMovement);
        }

        [PunRPC]
        protected void UpdateAnalogMoveRPC(bool inputAnalogMovement)
        {
            networkInputSync.inputAnalogMovement = inputAnalogMovement;
        }

        public void UpdateMove(Vector2 inputMove)
        {
            photonView.RPC("UpdateMoveRPC", RpcTarget.All, inputMove);
        }

        [PunRPC]
        protected void UpdateMoveRPC(Vector2 inputMove)
        {
            networkInputSync.inputMove = inputMove;
        }

        public void UpdateSprint(bool inputSprint)
        {
            photonView.RPC("UpdateSprintRPC", RpcTarget.AllBufferedViaServer, inputSprint);
        }

        [PunRPC]
        protected void UpdateSprintRPC(bool inputSprint)
        {
            networkInputSync.inputSprint = inputSprint;
        }

        public void UpdateLook(Vector2 inputLook)
        {
            photonView.RPC("UpdateLookRPC", RpcTarget.All, inputLook);
        }

        [PunRPC]
        protected void UpdateLookRPC(Vector2 inputLook)
        {
            networkInputSync.inputLook = inputLook;
        }
    }
}
#endif