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

        [SerializeField] public UnityEvent onSwitchedToFPS = new UnityEvent();
        [SerializeField] public UnityEvent onSwitchedToTPS = new UnityEvent();

        private IPlayerInfoProvider playerInfo;

        public bool IsFPS => CinemachineCore.Instance?.BrainCount > 0 && (
            CinemachineCore.Instance.GetActiveBrain(0)?.ActiveVirtualCamera
            ?.VirtualCameraGameObject.CompareTag("FPSVirtualCamera") ?? false);

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

        private void OnEnable()
        {
            
            if (IsFPS) onSwitchedToFPS.Invoke();
            else onSwitchedToTPS.Invoke();
            
            if (CinemachineCore.Instance.BrainCount == 0)
            {
                enabled = false;
                Debug.LogError("Cannot create player, no active Cinemachine brains found. Do you have a camera rig in your scene?");
                return;
            }
            
            var activeBrain = CinemachineCore.Instance.GetActiveBrain(0);
            if(activeBrain)
            {
                activeBrain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
            }
            else
            {
                enabled = false;
                Debug.LogError("Cannot create player, no Cinemachine brains found. Do you have a camera rig in your scene?");
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
        }

        public virtual void OnStartRemotePlayer()
        {
            name = $"Player {playerInfo.PlayerId} - {playerInfo.PlayerName}";
            HandleOwnableComponents(false);
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
