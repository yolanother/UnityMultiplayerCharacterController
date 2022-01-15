#if MIRROR
using System;
using Mirror;
using UnityEngine;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace DoubTech.Multiplayer.Input
{
    [RequireComponent(typeof(NetworkInputSync))]
    public class MirrorNetworkInputSync : NetworkBehaviour, IPlayerInputSync, IPlayerAnimSync
    {
        private NetworkInputSync networkInputSync;

        // Anim Sync
        [SyncVar] private float animSyncSpeed;
        [SyncVar] private bool animSyncIsGrounded;
        [SyncVar] private bool animSyncJump;
        [SyncVar] private bool animSyncFreeFall;
        [SyncVar] private float animSyncMotionSpeed;
        private double TOLERANCE = .0001f;

        private void Awake()
        {
            networkInputSync = GetComponent<NetworkInputSync>();
        }

        [Command(requiresAuthority = false)]
        public void UpdateCameraAngleServerRpc(float cameraAngle)
        {
            networkInputSync.cameraAngle = cameraAngle;
        }

        [Command(requiresAuthority = false)]
        public void UpdateJumpServerRpc(bool inputJump)
        {
            networkInputSync.inputJump = inputJump;
            networkInputSync.onJump?.Invoke();
        }

        [Command(requiresAuthority = false)]
        public void UpdateAnalogMoveServerRpc(bool inputAnalogMovement)
        {
            networkInputSync.inputAnalogMovement = inputAnalogMovement;
        }

        [Command(requiresAuthority = false)]
        public void UpdateMoveServerRpc(Vector2 inputMove)
        {
            networkInputSync.inputMove = inputMove;
        }

        [Command(requiresAuthority = false)]
        public void UpdateSprintServerRpc(bool inputSprint)
        {
            networkInputSync.inputSprint = inputSprint;
        }

        [Command(requiresAuthority = false)]
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

        [Command]
        public void SetAnimSyncJump(bool jump)
        {
            animSyncJump = jump;
        }

        public bool AnimSyncJump
        {
            get => animSyncJump;
            set
            {
                if(value != animSyncJump) SetAnimSyncJump(value);
            }
        }

        [Command(requiresAuthority = false)]
        private void SetAnimSyncFreeFall(bool freeFall)
        {
            animSyncFreeFall = freeFall;
        }
        
        public bool AnimSyncFreeFall
        {
            get => animSyncFreeFall;
            set { if(animSyncFreeFall != value) SetAnimSyncFreeFall(value); }
        }


        [Command(requiresAuthority = false)]
        private void SetAnimSyncSpeed(float speed)
        {
            animSyncSpeed = speed;
        }
        public float AnimSyncSpeed
        {
            get => animSyncSpeed;
            set
            {
                if(Math.Abs(animSyncSpeed - value) > TOLERANCE) SetAnimSyncSpeed(value);
            }
        }


        [Command(requiresAuthority = false)]
        private void SetAnimSyncMotionSpeed(float motionSpeed)
        {
            animSyncMotionSpeed = motionSpeed;
        }

        public float AnimSyncMotionSpeed
        {
            get => animSyncMotionSpeed;
            set
            {
                if(Math.Abs(animSyncMotionSpeed - value) > TOLERANCE) SetAnimSyncMotionSpeed(value);
            }
        }


        [Command(requiresAuthority = false)]
        private void SetAnimSyncIsGrounded(bool isGrounded)
        {
            animSyncIsGrounded = isGrounded;
        }

        public bool AnimSyncIsGrounded
        {
            get => animSyncIsGrounded;
            set
            {
                if(animSyncIsGrounded != value) SetAnimSyncIsGrounded(value);
            }
        }
    }
}
#endif