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

        [Header("Current Input State")]
        [SyncVar] public Vector2 syncedLook;
        [SyncVar] public bool syncedSprint;
        [SyncVar] public Vector2 syncedMove;
        [SyncVar] public bool syncedAnalogMovement;
        [SyncVar] public bool syncedJump;
        [SyncVar] public float syncedCameraAngle;
        private double TOLERANCE = .0001f;

        private void Awake()
        {
            networkInputSync = GetComponent<NetworkInputSync>();
        }

        [Command(requiresAuthority = false)]
        public void UpdateCameraAngleServerRpc(float cameraAngle)
        {
            syncedCameraAngle = cameraAngle;
        }

        [Command(requiresAuthority = false)]
        public void UpdateJumpServerRpc(bool inputJump)
        {
            syncedJump = inputJump;
            networkInputSync.onJump?.Invoke();
        }

        [Command(requiresAuthority = false)]
        public void UpdateAnalogMoveServerRpc(bool inputAnalogMovement)
        {
            syncedAnalogMovement = inputAnalogMovement;
        }

        [Command(requiresAuthority = false)]
        public void UpdateMoveServerRpc(Vector2 inputMove)
        {
            syncedMove = inputMove;
        }

        [Command(requiresAuthority = false)]
        public void UpdateSprintServerRpc(bool inputSprint)
        {
            syncedSprint = inputSprint;
        }

        [Command(requiresAuthority = false)]
        public void UpdateLookServerRpc(Vector2 inputLook)
        {
            syncedLook = inputLook;
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

        public float CameraAngle
        {
            get => syncedCameraAngle;
            set
            {
                if(Math.Abs(syncedCameraAngle - value) > TOLERANCE) UpdateCameraAngleServerRpc(value);
            }
        }

        public bool Jump
        {
            get => syncedJump;
            set
            {
                if(syncedJump != value) UpdateJumpServerRpc(value);
            }
        }
        public bool AnalogMovement
        {
            get => syncedAnalogMovement;
            set
            {
                if(syncedAnalogMovement != value) UpdateAnalogMoveServerRpc(value);
            }
        }
        public Vector2 Move
        {
            get => syncedMove;
            set
            {
                if(syncedMove != value) UpdateMoveServerRpc(value);
            }
        }
        public bool Sprint
        {
            get => syncedSprint;
            set
            {
                if(syncedSprint != value) UpdateSprintServerRpc(value);
            }
        }
        public Vector2 Look
        {
            get => syncedLook;
            set
            {
                if(syncedLook != value) UpdateLookServerRpc(value);
            }
        }
    }
}
#endif