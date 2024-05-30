using UnityEngine;

public class EnemyType3 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float enemyHealth = 50f;
    private Vector2 idleDestination;
    public float roamRadius = 5f;
    private CircleCollider2D detectionRadius; // Reference to the detection radius collider
    public bool isIdle = true;
    private GameObject player;
    private bool shouldMoveToSpawn = false;


    // Reference to the Spawn class for the boundary
    public Spawn spawn;

    void Start()
{
    SetNewIdleDestination();
    detectionRadius = GetComponentInChildren<CircleCollider2D>(); // Get the child collider

    // Find the GameObject named "SHEnemyType2" in the scene
    GameObject spawnGameObject = GameObject.FindWithTag("Type3Spawner");

    if (spawnGameObject != null)
    {
        // Get the Spawn component from the found GameObject
        spawn = spawnGameObject.GetComponent<Spawn>();

        if (spawn == null)
        {
            Debug.LogError("Spawn component not found on the GameObject named 'SHEnemyType2'!");
        }
    }
    else
    {
        Debug.LogError("GameObject named 'SHEnemyType2' not found in the scene!");
    }
}

    void Update()
    {
        if (player != null && !isIdle)
        {
            player playerScript = player.GetComponent<player>();
            Vector2 direction = (Vector2)(player.transform.position - transform.position);
            direction.Normalize();
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            if (shouldMoveToSpawn)
            {
                MoveToSpawn();
            }
            else
            {
                RoamAround();
            }
        }
    }

    void RoamAround()
    {
        // Move the enemy towards the idle destination
        Vector2 direction = (idleDestination - (Vector2)transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Check if the enemy has reached the idle destination
        if (Vector2.Distance(transform.position, idleDestination) < 0.1f)
        {
            // Set a new random idle destination within the roamRadius
            SetNewIdleDestination();
        }
    
        // Check if the enemy is outside the spawn boundary
        else if (IsOutsideBoundary(transform.position, spawn.transform.position, spawn.maxSpawnRadius))
        {
            // Move towards the spawn point
            shouldMoveToSpawn = true;
        }   
}

bool IsOutsideBoundary(Vector2 position, Vector2 boundaryCenter, float boundaryRadius)
{
    // Check if the position is outside the specified circular boundary
    return Vector2.Distance(position, boundaryCenter) > boundaryRadius;
}

    void SetNewIdleDestination()
    {
        idleDestination = (Vector2)transform.position + Random.insideUnitCircle * roamRadius;
        idleDestination.x = Mathf.Clamp(idleDestination.x, transform.position.x - roamRadius, transform.position.x + roamRadius);
        idleDestination.y = Mathf.Clamp(idleDestination.y, transform.position.y - roamRadius, transform.position.y + roamRadius);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isIdle = false;
            Debug.Log("Player detected!");
            player = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isIdle = true;
            Debug.Log("Player not detected!");
            player = null;

            // Move the enemy back to the spawn point
            shouldMoveToSpawn=true;
        }
    }

    void MoveToSpawn()
    {
        // Calculate the direction towards the spawn point
        Vector2 direction = (spawn.transform.position - (Vector3)transform.position).normalized;

        // Move the enemy towards the spawn point
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Check if the enemy has reached the spawn point
        if (Vector2.Distance(transform.position, spawn.transform.position) < 0.1f)
        {
            // Once at the spawn point, reset the flag and continue roaming around
            shouldMoveToSpawn = false;
            SetNewIdleDestination();
        }
    }
}