using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public static EnemySpawner instance;

    void Awake()
    {
        instance = this;
    }

    public void SpawnEnemies(int amount)
    {
        for (int i =0; i < amount; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawn.position, Quaternion.identity);
        }
    }

    public void SpawnRats(int amount)
    {
        Debug.Log("Ratten greifen die Ernte an!");
        SpawnEnemies(amount);
    }
}
