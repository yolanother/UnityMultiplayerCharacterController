using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.MCC
{
    public class Player : MonoBehaviour, INetworkPlayer
    {
        [SerializeField] private Transform fpsFollowTarget;
        [SerializeField] private Transform tpsFollowTarget;

        [SerializeField] private Behaviour[] localPlayerBehaviours;
        [SerializeField] private Behaviour[] remotePlayerBehaviours;

        [SerializeField] private GameObject[] localPlayerGameObjects;
        [SerializeField] private GameObject[] remotePlayerGameObjects;

        [SerializeField] private UnityEvent onAssignedLocalPlayer = new UnityEvent();
        [SerializeField] private UnityEvent onAssignedRemotePlayer = new UnityEvent();

        [SerializeField] public UnityEvent onSwitchedToFPS = new UnityEvent();
        [SerializeField] public UnityEvent onSwitchedToTPS = new UnityEvent();

        private IPlayerInfoProvider playerInfo;

        public bool IsFPS => Brain?.ActiveVirtualCamera
            ?.VirtualCameraGameObject.CompareTag("FPSVirtualCamera") ?? false;

        private IPlayerInfoProvider PlayerInfo
        {
            get
            {
                if (null == playerInfo)
                {
                    playerInfo = GetComponent<IPlayerInfoProvider>();
                }

                return playerInfo;
            }
        }

        private CinemachineBrain brain;
        private CinemachineBrain Brain
        {
            get
            {
                if (!brain)
                {
                    if (CinemachineCore.Instance.BrainCount == 0)
                    {
                        brain = FindObjectOfType<CinemachineBrain>();

                        if (!brain)
                        {
                            enabled = false;
                            Debug.LogError(
                                "Cannot create player, no active Cinemachine brains found. Do you have a camera rig in your scene?");
                        }
                    }
                    else
                    {
                        brain = CinemachineCore.Instance.GetActiveBrain(0);
                    }
                }

                return brain;
            }
        }

        private void OnEnable()
        {
            
            if (IsFPS) onSwitchedToFPS.Invoke();
            else onSwitchedToTPS.Invoke();
            
            var brain = Brain;
            if (brain)
            {
                brain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
            }
        }

        private void OnDisable()
        {
            if (CinemachineCore.Instance.BrainCount == 0) return;
            
            var activeBrain = CinemachineCore.Instance.GetActiveBrain(0);
            if (activeBrain)
            {
                CinemachineCore.Instance.GetActiveBrain(0).m_CameraActivatedEvent
                    .RemoveListener(OnCameraActivated);
            }
        }

        private void OnCameraActivated(ICinemachineCamera oldCamera, ICinemachineCamera newCamera)
        {
            if (IsFPS) onSwitchedToFPS.Invoke();
            else onSwitchedToTPS.Invoke();
        }
        
        public virtual void OnStartLocalPlayer()
        {
            name = $"Player {PlayerInfo.PlayerId} - {PlayerInfo.PlayerName}";
            AssignCamera("FPSVirtualCamera", fpsFollowTarget);
            AssignCamera("TPSVirtualCamera", tpsFollowTarget);
            HandleOwnableComponents(true);
            onAssignedLocalPlayer.Invoke();
        }

        public virtual void OnStartRemotePlayer()
        {
            name = $"Player {PlayerInfo.PlayerId} - {PlayerInfo.PlayerName}";
            HandleOwnableComponents(false);
            onAssignedRemotePlayer.Invoke();
        }

        public void HandleOwnableComponents(bool isOwned)
        {
            foreach (var component in localPlayerBehaviours)
            {
                component.enabled = isOwned;
            }

            foreach (var component in remotePlayerBehaviours)
            {
                component.enabled = !isOwned;
            }

            foreach (var localPlayerGameObject in localPlayerGameObjects)
            {
                localPlayerGameObject.SetActive(isOwned);
            }

            foreach (var localPlayerGameObject in remotePlayerGameObjects)
            {
                localPlayerGameObject.SetActive(!isOwned);
            }
        }

        private void AssignCamera(string virtualCameraName, Transform target)
        {
            var playerCameras = GameObject.FindGameObjectsWithTag(virtualCameraName);
            foreach (var playerCamera in playerCameras)
            {
                var virtualCamera = playerCamera.GetComponentInChildren<CinemachineVirtualCamera>();
                virtualCamera.Follow = target;
            }
        }
    }

    public interface INetworkPlayer
    {
        /// <summary>
        /// Called when the the player is created on the client. This is useful for activating local cameras etc.
        /// </summary>
        void OnStartLocalPlayer();
        
        /// <summary>
        /// Called when another player is created
        /// </summary>
        void OnStartRemotePlayer();
    }

    public interface IPlayerInfoProvider
    {
        string PlayerName { get; set; }
        uint PlayerId { get; }
        
        /// <summary>
        /// Is the current instance owned by the local player
        /// </summary>
        bool IsLocalPlayer { get; }
        
        /// <summary>
        /// Is the current instance of the player running on the server/host
        /// </summary>
        bool IsServer { get; }
    }
}
