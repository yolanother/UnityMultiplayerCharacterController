#if MIRROR
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace DoubTech.Networking.PlayerComponents
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private NetworkIdentity playerPrefab;
        [SerializeField] private PlayerSpawnPoint[] playerSpawnPoints;
        [SerializeField] private bool scanSceneForAdditionalSpawnPoints;

        public UnityEvent<ulong> onClientConnected = new UnityEvent<ulong>();
        public UnityEvent<ulong> onClientDisconnected = new UnityEvent<ulong>();
        public UnityEvent onClientSpawnComplete = new UnityEvent();
        public UnityEvent<ulong> onLocalSpawnComplete = new UnityEvent<ulong>();

        public UnityEvent onLocalClientConnected = new UnityEvent();

        public int PlayerCount => NetworkServer.connections.Count;

        protected List<PlayerSpawnPoint> sceneSpawnPoints = new List<PlayerSpawnPoint>();
        private HashSet<ulong> processedClients = new HashSet<ulong>();

        private void OnEnable()
        {
            if (null != playerSpawnPoints)
            {
                sceneSpawnPoints.AddRange(playerSpawnPoints);
            }

            if (scanSceneForAdditionalSpawnPoints)
            {
                sceneSpawnPoints.AddRange(FindObjectsOfType<PlayerSpawnPoint>());
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            StartCoroutine(SpawnAsync());
        }

        protected IEnumerator SpawnAsync()
        {
            yield return OnPrePlayerSpawn();

            yield return OnSpawnPlayer();

            yield return OnPostPlayerSpawn();

            OnClientSpawnCompleteClientRpc();
        }

        protected virtual IEnumerator OnSpawnPlayer()
        {
            var sceneSpawnPoints = OnGetSpawnPoints();
            var spawnPoint = sceneSpawnPoints[0];
            for (int i = 0; i < sceneSpawnPoints.Count; i++)
            {
                if (sceneSpawnPoints[i].IsAvailable)
                {
                    spawnPoint = sceneSpawnPoints[i];
                    break;
                }
            }

            var prefab = Instantiate(playerPrefab, spawnPoint.transform.position,
                spawnPoint.transform.rotation);
            //prefab.SpawnAsPlayerObject();


            yield return null;
        }

        protected virtual List<PlayerSpawnPoint> OnGetSpawnPoints()
        {
            return sceneSpawnPoints;
        }

        protected virtual IEnumerator OnPostPlayerSpawn()
        {
            yield return null;
        }

        protected virtual IEnumerator OnPrePlayerSpawn()
        {
            yield return null;
        }

        [ClientRpc]
        public void OnClientSpawnCompleteClientRpc()
        {
            //onClientSpawnComplete.Invoke();
            /*if (clientid == NetworkManager.LocalClientId)
            {
                onLocalSpawnComplete.Invoke(clientid);
            }*/
        }
    }
}
#endif