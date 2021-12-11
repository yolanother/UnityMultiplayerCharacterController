using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace DoubTech.Multiplayer
{
    public class ConnecitonUI : MonoBehaviour
    {
        [SerializeField] private Button serverButton;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        [SerializeField] private Button disconnectButton;

        private enum GameMode
        {
            Server,
            Host,
            Client
        }

        private GameMode mode;

        private bool initialized = false;

        private void Awake()
        {
            disconnectButton.gameObject.SetActive(false);
            serverButton.onClick.AddListener(() =>
            {
                Initialize();
                NetworkManager.singleton.StartServer();
                mode = GameMode.Server;
            });
            hostButton.onClick.AddListener(() =>
            {
                Initialize();
                NetworkManager.singleton.StartHost();
                mode = GameMode.Host;
            });
            clientButton.onClick.AddListener(() => {
                Initialize();
                NetworkManager.singleton.StartClient();
                mode = GameMode.Client;
            });
            disconnectButton.onClick.AddListener(() =>
            {
                Initialize();
                switch (mode)
                {
                    case GameMode.Client:
                        NetworkManager.singleton.StopClient();
                        break;
                    case GameMode.Host:
                        NetworkManager.singleton.StopHost();
                        break;
                    case GameMode.Server:
                        NetworkManager.singleton.StopServer();
                        break;
                }
                OnDisconnected();
            });
        }

        private void Initialize()
        {
            if (initialized) return;
            initialized = true;

            MCCNetworkManager.Singleton.OnServerStarted += OnConnected;
            MCCNetworkManager.Singleton.OnClientConnected += OnConnected;
            MCCNetworkManager.Singleton.OnClientDisconnected += OnDisconnected;
            MCCNetworkManager.Singleton.OnServerStopped += OnDisconnected;
        }

        private void OnDisconnected()
        {
            serverButton.gameObject.SetActive(true);
            hostButton.gameObject.SetActive(true);
            clientButton.gameObject.SetActive(true);
            disconnectButton.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void OnConnected()
        {
            serverButton.gameObject.SetActive(false);
            hostButton.gameObject.SetActive(false);
            clientButton.gameObject.SetActive(false);
            disconnectButton.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
