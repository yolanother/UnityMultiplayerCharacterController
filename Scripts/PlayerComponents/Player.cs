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

        [SerializeField] public UnityEvent onSwitchedToFPS = new UnityEvent();
        [SerializeField] public UnityEvent onSwitchedToTPS = new UnityEvent();

        private IPlayerInfoProvider playerInfo;

        public bool IsFPS => CinemachineCore.Instance?.GetActiveBrain(0)?.ActiveVirtualCamera
            ?.VirtualCameraGameObject?.CompareTag("FPSVirtualCamera") ?? false;

        private void Awake()
        {
            playerInfo = GetComponent<IPlayerInfoProvider>();
        }

        private void OnEnable()
        {
            if (IsFPS) onSwitchedToFPS.Invoke();
            else onSwitchedToTPS.Invoke();
            CinemachineCore.Instance.GetActiveBrain(0).m_CameraActivatedEvent
                .AddListener(OnCameraActivated);
        }

        private void OnDisable()
        {
            CinemachineCore.Instance.GetActiveBrain(0).m_CameraActivatedEvent
                .RemoveListener(OnCameraActivated);
        }

        private void OnCameraActivated(ICinemachineCamera oldCamera, ICinemachineCamera newCamera)
        {
            if (IsFPS) onSwitchedToFPS.Invoke();
            else onSwitchedToTPS.Invoke();
        }
        
        public virtual void OnStartLocalPlayer()
        {
            name = $"Player {playerInfo.PlayerId} - {playerInfo.PlayerName}";
            AssignCamera("FPSVirtualCamera", fpsFollowTarget);
            AssignCamera("TPSVirtualCamera", tpsFollowTarget);
        }

        public virtual void OnStartRemotePlayer()
        {
            name = $"Player {playerInfo.PlayerId} - {playerInfo.PlayerName}";
            DisableUnownedComponents();
        }

        public void DisableUnownedComponents()
        {
            if (!playerInfo.IsLocalPlayer)
            {
                foreach (var component in localPlayerBehaviours)
                {
                    component.enabled = false;
                }

                foreach (var component in remotePlayerBehaviours)
                {
                    component.enabled = true;
                }
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
