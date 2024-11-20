using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval;
    public float initialSpawnInterval;
    public float minSpawnInterval;
    public float spawnPositionX;
    public float spawnPositionY;

    private float timer;

    void Update()
    {
        spawnInterval = Mathf.Max(minSpawnInterval, initialSpawnInterval - LevelManager.Instance.score / 1000f);
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(spawnPositionX, spawnPositionY, 0f);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
