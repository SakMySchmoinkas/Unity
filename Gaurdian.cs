using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaurdian : MonoBehaviour
{
    private float timeBtwShots;
    public float startTimeBtwShots;
    private Transform player;
    private CircleCollider2D detectionCollider;
    public GameObject EnemySnowBallPrefab;
    public float snowballDamageAmount = 10f;

    // Start is called before the first frame update
    void Start()
    {
        timeBtwShots = startTimeBtwShots;
        // Assuming the detection collider is a child of the Guardian
        detectionCollider = GetComponentInChildren<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && player.CompareTag("Player"))
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionCollider.radius)
            {
                // Player is within detection radius

                if (timeBtwShots <= 0)
                {
                    // Instantiate the EnemySnowBall
                    GameObject enemySnowBall = Instantiate(EnemySnowBallPrefab, transform.position, Quaternion.identity);

                    // Set the damage amount on the EnemySBScript of the instantiated SnowBall
                    EnemySBScript snowballScript = enemySnowBall.GetComponent<EnemySBScript>();
                    if (snowballScript != null)
                    {
                        snowballScript.damageAmount = 10;
                    }

                    timeBtwShots = startTimeBtwShots;
                }
                else
                {
                    timeBtwShots -= Time.deltaTime;
                }
            }
        }
    }

    // Set the player reference when it enters the detection radius
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
        }
    }

    // Clear the player reference when it exits the detection radius
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }
}
