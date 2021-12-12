using System;
using System.Collections;
using DoubTech.Multiplayer;
using Mirror;
using UnityEngine;

public abstract class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        NetworkClient.RegisterPrefab(playerPrefab);
    }

    public void Spawn(NetworkConnection conn, Action<GameObject> onPlayerSpawned)
    {
        Debug.Log($"Spawn for {conn}");
        StartCoroutine(SpawnAsync(conn, onPlayerSpawned));
    }

    protected IEnumerator SpawnAsync(NetworkConnection conn, Action<GameObject> onPlayerSpawned)
    {
        yield return OnPrePlayerSpawn();

        yield return OnSpawnPlayer(conn, onPlayerSpawned);

        yield return OnPostPlayerSpawn();

        OnClientSpawnCompleted();
    }

    protected virtual IEnumerator OnSpawnPlayer(NetworkConnection conn, Action<GameObject> onPlayerSpawned)
    {
        var transform = OnGetSpawnPoint();
        Debug.Log($"Spawning {playerPrefab.name} at {transform.position}");
        var player = Instantiate(playerPrefab, transform.position, transform.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
        onPlayerSpawned?.Invoke(player);

        yield return null;
    }

    protected abstract Transform OnGetSpawnPoint();

    protected virtual IEnumerator OnPostPlayerSpawn()
    {
        yield return null;
    }

    protected virtual IEnumerator OnPrePlayerSpawn()
    {
        yield return null;
    }

    protected virtual void OnClientSpawnCompleted()
    {

    }
}
