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

        [Header("Current Input State")]
        public Vector2 syncedLook;
        public bool syncedSprint;
        public Vector2 syncedMove;
        public bool syncedAnalogMovement;
        public bool syncedJump;
        public float syncedCameraAngle;

        private void Awake()
        {
            networkInputSync = GetComponent<NetworkInputSync>();
        }

        public void UpdateCameraAngle(float cameraAngle)
        {
            if (Math.Abs(cameraAngle - syncedCameraAngle) > TOLERANCE)
            {
                photonView.RPC("UpdateCameraAngleRPC", RpcTarget.AllBufferedViaServer, cameraAngle);
            }
        }

        [PunRPC]
        protected void UpdateCameraAngleRPC(float cameraAngle)
        {
            syncedCameraAngle = cameraAngle;
        }

        public void UpdateJump(bool inputJump)
        {
            if (inputJump != syncedJump)
            {
                photonView.RPC("UpdateJumpRPC", RpcTarget.AllBufferedViaServer, inputJump);
            }
        }

        [PunRPC]
        protected void UpdateJumpRPC(bool inputJump)
        {
            syncedJump = inputJump;
            networkInputSync.onJump?.Invoke();
        }

        public void UpdateAnalogMove(bool inputAnalogMovement)
        {
            if (inputAnalogMovement != syncedAnalogMovement)
            {
                photonView.RPC("UpdateAnalogMoveRPC", RpcTarget.AllBufferedViaServer, inputAnalogMovement);
            }
        }

        [PunRPC]
        protected void UpdateAnalogMoveRPC(bool inputAnalogMovement)
        {
            syncedAnalogMovement = inputAnalogMovement;
        }

        public void UpdateMove(Vector2 inputMove)
        {
            if (inputMove != syncedMove)
            {
                photonView.RPC("UpdateMoveRPC", RpcTarget.AllBufferedViaServer, inputMove);
            }
        }

        [PunRPC]
        protected void UpdateMoveRPC(Vector2 inputMove)
        {
            syncedMove = inputMove;
        }

        public void UpdateSprint(bool inputSprint)
        {
            if (inputSprint != syncedSprint)
            {
                photonView.RPC("UpdateSprintRPC", RpcTarget.AllBufferedViaServer, inputSprint);
            }
        }

        [PunRPC]
        protected void UpdateSprintRPC(bool inputSprint)
        {
            syncedSprint = inputSprint;
        }

        public void UpdateLook(Vector2 inputLook)
        {
            if (inputLook != syncedLook)
            {
                photonView.RPC("UpdateLookRPC", RpcTarget.AllBufferedViaServer, inputLook);
            }
        }

        [PunRPC]
        protected void UpdateLookRPC(Vector2 inputLook)
        {
            syncedLook = inputLook;
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

        public float CameraAngle
        {
            get => syncedCameraAngle;
            set
            {
                if(Math.Abs(syncedCameraAngle - value) > TOLERANCE) UpdateCameraAngle(value);
            }
        }

        public bool Jump
        {
            get => syncedJump;
            set
            {
                if(syncedJump != value) UpdateJump(value);
            }
        }
        public bool AnalogMovement
        {
            get => syncedAnalogMovement;
            set
            {
                if(syncedAnalogMovement != value) UpdateAnalogMove(value);
            }
        }
        public Vector2 Move
        {
            get => syncedMove;
            set
            {
                if(syncedMove != value) UpdateMove(value);
            }
        }
        public bool Sprint
        {
            get => syncedSprint;
            set
            {
                if(syncedSprint != value) UpdateSprint(value);
            }
        }
        public Vector2 Look
        {
            get => syncedLook;
            set
            {
                if(syncedLook != value) UpdateLook(value);
            }
        }
    }
}
#endif