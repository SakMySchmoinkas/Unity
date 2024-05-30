using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : MonoBehaviour
{
    public float speed;
    public float detectionRadius; // Circle Collider for detection
    private Transform player;

    private Vector2 idleDestination;
    public float roamRadius;

    private bool isIdle = true; // Define isIdle variable
    private bool shouldMoveToSpawn; // Define shouldMoveToSpawn variable

    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    public float knockbackDistance = 2f;
    public LayerMask obstacleLayer;
    private bool isKnockedBack = false;
    public float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            // If player is not found, look for an object with the tag "Undetectable"
            playerObject = GameObject.FindGameObjectWithTag("Undetectable");

            // Check if "Undetectable" object is found
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                // Handle the case when neither "Player" nor "Undetectable" is found
                Debug.LogError("Both Player and Undetectable objects not found!");
            }
        }

        SetNewIdleDestination();
    }

    void Update()
{
    if (player != null)
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius && player.CompareTag("Player"))
        {
            // Player is within detection range, start following
            isIdle = false;

            // Use correct variable name: speed instead of moveSpeed
            Vector2 direction = (Vector2)(player.position - transform.position);
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime);
        }
        else
        {
            // Player is outside detection range, enter idle state
            isIdle = true;

            if (shouldMoveToSpawn)
            {
                // Implement MoveToSpawn() method or remove the reference
            }
            else
            {
                RoamAround();
            }
        }
    }
}


    void RoamAround()
    {
        if (Vector2.Distance(transform.position, idleDestination) < 0.1f)
        {
            SetNewIdleDestination();
        }

        Vector2 direction = (idleDestination - (Vector2)transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void SetNewIdleDestination()
    {
        idleDestination = (Vector2)transform.position + Random.insideUnitCircle * roamRadius;
        idleDestination.x = Mathf.Clamp(idleDestination.x, transform.position.x - roamRadius, transform.position.x + roamRadius);
        idleDestination.y = Mathf.Clamp(idleDestination.y, transform.position.y - roamRadius, transform.position.y + roamRadius);
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

    // Public method to start knockback from other scripts
    public void TriggerKnockback(Vector2 knockbackDirection)
    {
        StartCoroutine(Knockback(knockbackDirection));
    }

    public Vector2 GetDirection()
    {
        return isIdle ? (idleDestination - (Vector2)transform.position).normalized : ((Vector2)(player.position - transform.position)).normalized;
    }   
}
