#if PUN_2_0_OR_NEWER
using DoubTech.Multiplayer;
using Photon.Pun;
using UnityEngine;

namespace DoubTech.Networking.Pun
{
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    [RequireComponent(typeof(INetworkPlayer))]
    public class PunPlayer : MonoBehaviourPunCallbacks, IPlayerInfoProvider
    {
        private string playerName;
        
        private INetworkPlayer networkPlayer;

        // Start is called before the first frame update
        void Awake()
        {
            networkPlayer = GetComponent<INetworkPlayer>();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (PhotonNetwork.InRoom)
            {
                OnJoinedRoom();
            }
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            if (photonView.IsMine)
            {
                networkPlayer.OnStartLocalPlayer();
            }
            else
            {
                networkPlayer.OnStartRemotePlayer();
            }
        }

        public string PlayerName
        {
            get => photonView.Owner.NickName;
            set
            {
                if (photonView.IsMine)
                {
                    PhotonNetwork.LocalPlayer.NickName = value;
                }
            }
        }

        public uint PlayerId => (uint) photonView.OwnerActorNr;
        public bool IsLocalPlayer => photonView.IsMine;
        public bool IsServer => photonView.IsMine;
    }
}
#endif