#if MIRROR
using DoubTech.Multiplayer;
using Mirror;
using UnityEngine;

namespace DoubTech.Networking.Mirror
{
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(INetworkPlayer))]
    public class MirrorPlayer : NetworkBehaviour, IPlayerInfoProvider
    {
        [SyncVar] private string playerName;
        
        private INetworkPlayer networkPlayer;

        // Start is called before the first frame update
        void Awake()
        {
            networkPlayer = GetComponent<INetworkPlayer>();
            if (isServer)
            {
                playerName = "Player " + netId.ToString();
            }
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            networkPlayer.OnStartLocalPlayer();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            networkPlayer.OnStartRemotePlayer();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!isLocalPlayer)
            {
                networkPlayer.OnStartRemotePlayer();
            }
        }

        [Command]
        void SetPlayerName(string name)
        {
            playerName = name;
        }

        public string PlayerName
        {
            get => playerName;
            set => SetPlayerName(value);
        }

        public uint PlayerId => netId;
        public bool IsLocalPlayer => isLocalPlayer;
        public bool IsServer => isServer;
    }
}
#endif