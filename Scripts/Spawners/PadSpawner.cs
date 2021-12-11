using System.Collections.Generic;
using DoubTech.Networking.PlayerComponents;
using Mirror;
using UnityEngine;

public class PadSpawner : PlayerSpawner
{
    [SerializeField] private PlayerSpawnPoint[] playerSpawnPoints;
    [SerializeField] private bool scanSceneForAdditionalSpawnPoints;

    protected List<PlayerSpawnPoint> sceneSpawnPoints = new List<PlayerSpawnPoint>();

    public List<PlayerSpawnPoint> SpawnPoints
    {
        get
        {
            if (null != playerSpawnPoints)
            {
                sceneSpawnPoints.AddRange(playerSpawnPoints);
            }

            if (scanSceneForAdditionalSpawnPoints)
            {
                sceneSpawnPoints.AddRange(FindObjectsOfType<PlayerSpawnPoint>());
            }

            return sceneSpawnPoints;
        }
    }

    protected override Transform OnGetSpawnPoint()
    {
        var sceneSpawnPoints = SpawnPoints;
        var spawnPoint = sceneSpawnPoints[0];
        for (int i = 0; i < sceneSpawnPoints.Count; i++)
        {
            if (sceneSpawnPoints[i].IsAvailable)
            {
                spawnPoint = sceneSpawnPoints[i];
                break;
            }
        }

        return spawnPoint.transform;
    }
}
