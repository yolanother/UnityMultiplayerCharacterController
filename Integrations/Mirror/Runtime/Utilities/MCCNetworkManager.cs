#if MIRROR
using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace DoubTech.Multiplayer
{
    public class MCCNetworkManager : NetworkManager
    {
        public static MCCNetworkManager Singleton => (MCCNetworkManager) singleton;

        public HostSpawner hostSpawner;
        public PlayerSpawner playerSpawner;

        public event Action OnServerStarted;
        public event Action OnServerStopped;

        public event Action OnClientConnected;
        public event Action OnClientDisconnected;

        public event Action<NetworkConnection, GameObject> OnPlayerSpawned;

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            playerSpawner.Spawn(conn, p => OnPlayerSpawned?.Invoke(conn, p));
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            OnClientDisconnected?.Invoke();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            OnServerStarted?.Invoke();
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            OnServerStopped?.Invoke();
        }
    }
}
#endif