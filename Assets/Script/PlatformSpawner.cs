using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;

    public float spawnInterval;
    public float initialSpawnInterval;
    public float minSpawnInterval;
    public float spawnPositionX;
    public float minY;
    public float maxY;

    private float timer;

    void Update()
    {
        spawnInterval = Mathf.Max(minSpawnInterval, initialSpawnInterval - LevelManager.Instance.score / 1000f);
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnPlatform();
            timer = 0f;
        }
    }

    void SpawnPlatform()
    {
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnPositionX, randomY, 0f);
        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
    }
}
