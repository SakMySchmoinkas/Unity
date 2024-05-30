using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampSpawner : MonoBehaviour
{
    public List<GameObject> objectsToSpawn;
    public List<GameObject> EnemyToSpawn;
    public int numberOfObjectsPerType = 5;
    public float minSpawnRadius = 5f;
    public float maxSpawnRadius = 10f;
    public int spawnDelay = 1;
    public float minDistanceFromOther = 2f;
    private int maxAttempts = 200; // Maximum attempts to find a valid position
    private float randomAngle;
    private float randomRadius;
    private Vector2 randomPosition;
    public int j = 0;

    void Start()
    {
        Invoke("SpawnObjects", spawnDelay);
    }

    void SpawnObjects()
    {
        foreach (GameObject prefab in objectsToSpawn)
        {
            for (int i = 0; i < numberOfObjectsPerType; i++)
            {
                bool positionIsValid = false;
                int attempts = 0;

                while (!positionIsValid && attempts < maxAttempts)
                {
                    // Generate a random angle
                    float randomAngle = Random.Range(0f, 2f * Mathf.PI);

                    // Generate a random radius within the specified range
                    float randomRadius = Random.Range(minSpawnRadius, maxSpawnRadius);

                    // Calculate the position using polar coordinates
                    Vector2 newPosition = new Vector2(Mathf.Cos(randomAngle) * randomRadius, Mathf.Sin(randomAngle) * randomRadius);

                    // Check if the new position is too close to existing objects
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(newPosition, minDistanceFromOther);
                    positionIsValid = true;

                    foreach (Collider2D collider in colliders)
                    {
                        // Check if the collider belongs to a spawned object
                        if (collider.gameObject.CompareTag("SpawnedObject"))
                        {
                            positionIsValid = false;
                            break;
                        }
                    }

                    if (positionIsValid)
                    {
                        // Instantiate the object at the valid position
                        GameObject spawnedObject = Instantiate(prefab, newPosition, Quaternion.identity);
                        spawnedObject.tag = "SpawnedObject"; // Set a tag to identify spawned objects

                        // Optionally spawn an enemy if EnemyToSpawn list is not empty
                        if (EnemyToSpawn.Count > 0)
                        {
                            Instantiate(EnemyToSpawn[j], newPosition, Quaternion.identity);
                        }
                    }
                    else
                    {
                        attempts++;
                    }
                }
            }
            j++;
        }
    }
}
