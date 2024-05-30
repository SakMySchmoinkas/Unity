using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public float minSpawnInterval = 5f;
    public float maxSpawnInterval = 10f;
    public float minSpawnRadius = 10f;
    public float maxSpawnRadius = 30f;
    public int maxEnemyCount = 5;
    public int ItemIndex = 0;
    public List<GameObject> gameObjectsList = new List<GameObject>();

    private void Start()
    {
        // Start the coroutine to spawn enemies after a delay of 5 seconds
        StartCoroutine(DelayedSpawn());
    }

    private IEnumerator DelayedSpawn()
    {
        // Wait for 5 seconds before starting the SpawnEnemy coroutine
        yield return new WaitForSeconds(5f);

        // Start the coroutine to spawn enemies periodically
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {

            ItemIndex = Random.Range(0, gameObjectsList.Count);

            // Check the current number of enemies in the scene
            int currentEnemyCount = CountEnemyPrefabs();

            // If the current number of enemies is less than the maximum allowed, spawn a new enemy
            if (currentEnemyCount < maxEnemyCount)
            {
                // Calculate a random spawn interval between min and max values
                float randomSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

                // Calculate a random distance within the specified radius range
                float randomSpawnRadius = Random.Range(minSpawnRadius, maxSpawnRadius);

                // Calculate a random angle to determine the spawn position
                float randomAngle = Random.Range(0f, 360f);
                Vector2 spawnDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
                Vector3 spawnPosition = (Vector3)spawnDirection * randomSpawnRadius + transform.position;

                // Spawn the enemy at the calculated position
                Instantiate(gameObjectsList[ItemIndex], spawnPosition, Quaternion.identity);
            }

            // Wait for the randomly calculated spawn interval
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            
        }
        
    }

    private int CountEnemyPrefabs()
    {
        // Find all instances of the enemyPrefab in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(gameObjectsList[ItemIndex].tag);

        // Return the count of enemyPrefabs
        return enemies.Length;
    }

    void SpawnUpdate(){


    }
}