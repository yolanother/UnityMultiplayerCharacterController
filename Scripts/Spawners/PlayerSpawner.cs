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

    public void Spawn(NetworkConnection conn)
    {
        Debug.Log($"Spawn for {conn}");
        StartCoroutine(SpawnAsync(conn));
    }

    protected IEnumerator SpawnAsync(NetworkConnection conn)
    {
        yield return OnPrePlayerSpawn();

        yield return OnSpawnPlayer(conn);

        yield return OnPostPlayerSpawn();

        OnClientSpawnCompleted();
    }

    protected virtual IEnumerator OnSpawnPlayer(NetworkConnection conn)
    {
        var transform = OnGetSpawnPoint();
        Debug.Log($"Spawning {playerPrefab.name} at {transform.position}");
        var player = Instantiate(playerPrefab, transform.position, transform.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);

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
