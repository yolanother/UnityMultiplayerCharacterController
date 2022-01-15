#if PUN_2_0_OR_NEWER
using System;
using Photon.Pun;
using UnityEngine;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace DoubTech.Multiplayer.Input
{
    [RequireComponent(typeof(NetworkInputSync))]
    public class PunNetworkInputSync : MonoBehaviourPun, IPlayerInputSync, IPlayerAnimSync
    {
        private const double TOLERANCE = 0.0001;
        private NetworkInputSync networkInputSync;
        private float animSyncSpeed;
        private bool animSyncIsGrounded;
        private bool animSyncJump;
        private bool animSyncFreeFall;
        private float animSyncMotionSpeed;

        private void Awake()
        {
            networkInputSync = GetComponent<NetworkInputSync>();
        }

        public void UpdateCameraAngle(float cameraAngle)
        {
            if (Math.Abs(cameraAngle - networkInputSync.cameraAngle) > TOLERANCE)
            {
                photonView.RPC("UpdateCameraAngleRPC", RpcTarget.AllBufferedViaServer, cameraAngle);
            }
        }

        [PunRPC]
        protected void UpdateCameraAngleRPC(float cameraAngle)
        {
            networkInputSync.cameraAngle = cameraAngle;
        }

        public void UpdateJump(bool inputJump)
        {
            if (inputJump != networkInputSync.inputJump)
            {
                photonView.RPC("UpdateJumpRPC", RpcTarget.AllBufferedViaServer, inputJump);
            }
        }

        [PunRPC]
        protected void UpdateJumpRPC(bool inputJump)
        {
            networkInputSync.inputJump = inputJump;
            networkInputSync.onJump?.Invoke();
        }

        public void UpdateAnalogMove(bool inputAnalogMovement)
        {
            if (inputAnalogMovement != networkInputSync.inputAnalogMovement)
            {
                photonView.RPC("UpdateAnalogMoveRPC", RpcTarget.AllBufferedViaServer, inputAnalogMovement);
            }
        }

        [PunRPC]
        protected void UpdateAnalogMoveRPC(bool inputAnalogMovement)
        {
            networkInputSync.inputAnalogMovement = inputAnalogMovement;
        }

        public void UpdateMove(Vector2 inputMove)
        {
            if (inputMove != networkInputSync.inputMove)
            {
                photonView.RPC("UpdateMoveRPC", RpcTarget.AllBufferedViaServer, inputMove);
            }
        }

        [PunRPC]
        protected void UpdateMoveRPC(Vector2 inputMove)
        {
            networkInputSync.inputMove = inputMove;
        }

        public void UpdateSprint(bool inputSprint)
        {
            if (inputSprint != networkInputSync.inputSprint)
            {
                photonView.RPC("UpdateSprintRPC", RpcTarget.AllBufferedViaServer, inputSprint);
            }
        }

        [PunRPC]
        protected void UpdateSprintRPC(bool inputSprint)
        {
            networkInputSync.inputSprint = inputSprint;
        }

        public void UpdateLook(Vector2 inputLook)
        {
            if (inputLook != networkInputSync.inputLook)
            {
                photonView.RPC("UpdateLookRPC", RpcTarget.AllBufferedViaServer, inputLook);
            }
        }

        [PunRPC]
        protected void UpdateLookRPC(Vector2 inputLook)
        {
            networkInputSync.inputLook = inputLook;
        }


        [PunRPC]
        private void SetAnimSyncJumpRPC(bool jump)
        {
            animSyncJump = jump;
        }
        public bool AnimSyncJump
        {
            get => animSyncJump;
            set
            {
                if (animSyncJump != value) photonView.RPC("SetAnimSyncJumpRPC", RpcTarget.AllBufferedViaServer, value);
            }
        }

        [PunRPC]
        private void SetAnimSyncFreeFallRPC(bool isFreeFall)
        {
            animSyncFreeFall = isFreeFall;
        }
        public bool AnimSyncFreeFall
        {
            get => animSyncFreeFall;
            set
            {
                if (animSyncFreeFall != value) photonView.RPC("SetAnimSyncFreeFallRPC", RpcTarget.AllBufferedViaServer, value);
            }
        }

        [PunRPC]
        private void SetAnimSyncSpeedRPC(float speed)
        {
            animSyncSpeed = speed;
        }
        public float AnimSyncSpeed
        {
            get => animSyncSpeed;
            set
            {
                if (Math.Abs(animSyncSpeed - value) > TOLERANCE) photonView.RPC("SetAnimSyncSpeedRPC", RpcTarget.AllBufferedViaServer, value);
            }
        }
        
        [PunRPC]
        private void SetAnimSyncMotionSpeedRPC(float motionSpeed)
        {
            animSyncMotionSpeed = motionSpeed;
        }
        public float AnimSyncMotionSpeed
        {
            get => animSyncMotionSpeed;
            set
            {
                if (Math.Abs(animSyncMotionSpeed - value) > TOLERANCE)
                {
                    photonView.RPC("SetAnimSyncMotionSpeedRPC", RpcTarget.AllBufferedViaServer, value);
                }
            }
        }

        [PunRPC]
        private void SetAnimSyncIsGroundedRPC(bool isGrounded)
        {
            animSyncIsGrounded = isGrounded;
        }
        
        public bool AnimSyncIsGrounded
        {
            get => animSyncIsGrounded;
            set
            {
                if (animSyncIsGrounded != value) photonView.RPC("SetAnimSyncIsGroundedRPC", RpcTarget.AllBufferedViaServer, value);
            }
        }
    }
}
#endif