using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.Multiplayer
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private Transform fpsFollowTarget;
        [SerializeField] private Transform tpsFollowTarget;

        [SerializeField] private Behaviour[] localPlayerBehaviours;
        [SerializeField] private Behaviour[] remotePlayerBehaviours;

        [SerializeField] public UnityEvent onSwitchedToFPS = new UnityEvent();
        [SerializeField] public UnityEvent onSwitchedToTPS = new UnityEvent();

        public bool IsFPS => CinemachineCore.Instance?.GetActiveBrain(0)?.ActiveVirtualCamera
            ?.VirtualCameraGameObject?.CompareTag("FPSVirtualCamera") ?? false;

        private void OnEnable()
        {
            if (IsFPS) onSwitchedToFPS.Invoke();
            else onSwitchedToTPS.Invoke();
            CinemachineCore.Instance.GetActiveBrain(0).m_CameraActivatedEvent
                .AddListener(OnCameraActivated);
            DisableUnownedComponents();
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

        public override void OnStartClient()
        {
            Debug.Log($"OnStartClient: {netIdentity.netId}");
            base.OnStartClient();
            name = $"Player {netIdentity.netId}";
        }

        public override void OnStartServer()
        {
            Debug.Log($"OnStartServer: {netIdentity.netId}");
            base.OnStartServer();
        }

        public override void OnStartAuthority()
        {
            Debug.Log($"OnStartAuthority: {netIdentity.netId}");
            base.OnStartAuthority();
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            name = $"Player {netIdentity.netId}";
            Debug.Log($"Staring local player: {name}");
            AssignCamera("FPSVirtualCamera", fpsFollowTarget);
            AssignCamera("TPSVirtualCamera", tpsFollowTarget);
        }

        public void DisableUnownedComponents()
        {
            if (!isLocalPlayer)
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
}
