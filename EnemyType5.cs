using System.Collections;
using UnityEngine;

public class EnemyType5 : MonoBehaviour
{
    public float moveSpeed;
    public float stoppingDistance;
    public float retreatDistance;

    public float detectionRadius;
    public GameObject EnemySnowBall;
    public Transform player;

    private float timeBtwShots;
    public float startTimeBtwShots;

    private Vector2 idleDestination;
    public float roamRadius;
    public bool shouldMoveToSpawn = true;
    public bool isIdle = true;
    public string campTag;

    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    public float knockbackDistance = 2f;
    public LayerMask obstacleLayer;
    private bool isKnockedBack = false;
    public float distanceToPlayer;

    // Reference to the Spawn class for the boundary
    public Spawn spawn;
    public Transform spawnTransform;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Invoke("ChangeTag", 2f);

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            playerObject = GameObject.FindGameObjectWithTag("Undetectable");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        // Set the spawnTransform to the current position of the enemy
        spawnTransform = transform;

        timeBtwShots = startTimeBtwShots;
        SetNewIdleDestination();
    }

    void Update()
    {
        if (player != null && player.CompareTag("Player"))
        {
            distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRadius)
            {
                // Player is within detection radius
                if (distanceToPlayer > stoppingDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                }
                else if (distanceToPlayer < stoppingDistance && distanceToPlayer > retreatDistance)
                {
                    transform.position = this.transform.position;
                }
                else if (distanceToPlayer < retreatDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.position, -moveSpeed * Time.deltaTime);
                }

                if (timeBtwShots <= 0)
                {
                    Instantiate(EnemySnowBall, transform.position, Quaternion.identity);
                    timeBtwShots = startTimeBtwShots;
                }
                else
                {
                    timeBtwShots -= Time.deltaTime;
                }
            }
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
        Vector2 direction = (idleDestination - (Vector2)transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, idleDestination) < 0.1f)
        {
            SetNewIdleDestination();
        }
        else if (IsOutsideBoundary(transform.position, spawnTransform.position, roamRadius))
        {
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
        if (other.gameObject.CompareTag("Player"))
        {
            isIdle = false;
            Debug.Log("Player detected!");
            player = other.gameObject.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isIdle = true;
            Debug.Log("Player not detected!");
            player = null;

            // Move the enemy back to the spawn point
            shouldMoveToSpawn = true;
        }
    }

    void MoveToSpawn()
    {
        Vector2 direction = (spawnTransform.position - (Vector3)transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, spawnTransform.position) < 0.1f)
        {
            shouldMoveToSpawn = false;
            SetNewIdleDestination();
        }
    }

   void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;

            StartCoroutine(Knockback(knockbackDirection));
        }
    }

    IEnumerator Knockback(Vector2 knockbackDirection)
    {
        if (!isKnockedBack)
        {
            isKnockedBack = true;

            Vector2 initialPosition = transform.position;
            Vector2 knockbackLocation = initialPosition + knockbackDirection * knockbackDistance;

            GetComponent<Rigidbody2D>().isKinematic = true;

            float elapsedTime = 0f;

            RaycastHit2D hit = Physics2D.Raycast(initialPosition, knockbackDirection, knockbackDistance + 1, obstacleLayer);
            if (hit.collider != null)
            {
                knockbackLocation = hit.point - knockbackDirection * 0.1f;
            }

            while (elapsedTime < knockbackDuration)
            {
                transform.position = Vector2.Lerp(initialPosition, knockbackLocation, elapsedTime / knockbackDuration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = knockbackLocation;

            GetComponent<Rigidbody2D>().isKinematic = false;

            isKnockedBack = false;
        }
    }

    public void TriggerKnockback(Vector2 knockbackDirection)
    {
        StartCoroutine(Knockback(knockbackDirection));
    }

    public Vector2 GetDirection()
    {
        return isIdle ? (idleDestination - (Vector2)transform.position).normalized : ((Vector2)(player.position - transform.position)).normalized;
    }

    void ChangeTag()
    {
        // Change the tag of the GameObject to your desired tag
        gameObject.tag = "EnemyType5";
    }
}
