using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType4 : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;

    private CircleCollider2D detectionCollider; // Circle Collider for detection

    public GameObject EnemySnowBall;
    private Transform player;

    private float timeBtwShots;
    public float startTimeBtwShots;

    private Vector2 idleDestination;
    public float roamRadius;

    // Start is called before the first frame update
    void Start()
{
    detectionCollider = GetComponentInChildren<CircleCollider2D>(); // Assuming the detection collider is a child

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

    timeBtwShots = startTimeBtwShots;
    
}

    // Update is called once per frame
    void Update()
    {
        if (player.CompareTag("Player"))
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionCollider.radius)
            {
                // Player is within detection radius
                if (distanceToPlayer > stoppingDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                }
                else if (distanceToPlayer < stoppingDistance && distanceToPlayer > retreatDistance)
                {
                    transform.position = this.transform.position;
                }
                else if (distanceToPlayer < retreatDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
                }

                if (timeBtwShots < -0)
                {
                    if (player != null && player.CompareTag("Player"))
                    {
                        Instantiate(EnemySnowBall, transform.position, Quaternion.identity);
                        timeBtwShots = startTimeBtwShots;
                    }
                }
                else
                {
                    timeBtwShots -= Time.deltaTime;
                }
            }
        }
    }
}