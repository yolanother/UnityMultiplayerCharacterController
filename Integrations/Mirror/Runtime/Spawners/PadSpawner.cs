using System.Collections.Generic;
using DoubTech.Networking.PlayerComponents;
using UnityEngine;

public class PadSpawner : MonoBehaviour
{
    [SerializeField] private PlayerSpawnPoint[] playerSpawnPoints;
    [SerializeField] private bool scanSceneForAdditionalSpawnPoints;


    public List<PlayerSpawnPoint> SpawnPoints
    {
        get
        {
            List <PlayerSpawnPoint> sceneSpawnPoints = new List< PlayerSpawnPoint>();
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

    public Transform OnGetSpawnPoint()
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
