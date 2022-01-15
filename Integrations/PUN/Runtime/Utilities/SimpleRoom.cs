#if PUN_2_0_OR_NEWER
using Photon.Pun;
using UnityEngine;

namespace DoubTech.Multiplayer.PUN
{
    public class SimpleRoom : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject photonPlayer;
        
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master. Joining room...");
            base.OnConnectedToMaster();
            PhotonNetwork.JoinRandomOrCreateRoom();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined room.");
            base.OnJoinedRoom();
            PhotonNetwork.Instantiate(photonPlayer.name, Vector3.zero, Quaternion.identity);
        }
    }
}
#endif