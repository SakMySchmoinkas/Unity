using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EnemySBScript : MonoBehaviour
{
    public float speed = 10f;
    public float damageAmount = 5f;
    public float range = 5f;
    private Vector2 initialPosition;
    private Transform playerObject;
    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player").transform;
        direction = (playerObject.position - transform.position).normalized;
        initialPosition = transform.position;
    }

    void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;

        if (Vector2.Distance(initialPosition, transform.position) >= range)
        {
            Destroy(gameObject);
            
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider has the tag "Enemy"
        if (other.tag.Contains("Player"))
        {
            // Get the enemy's health component
            player playerScript = other.GetComponent<player>();

            // Check if the enemy has a health component
            if (playerScript != null)
            {
                // Reduce the player's health by the specified amount
                playerScript.health-=damageAmount;

                // Trigger knockback on the enemy
                TriggerKnockback(other.gameObject);
            }

            // Destroy the bullet game object
            Destroy(gameObject);
        }
    }

    private void TriggerKnockback(GameObject player1)
    {
        // Get the EnemyType1 script from the enemy GameObject
        player playerScript = player1.GetComponent<player>();

        // Check if the enemy has the EnemyType1 script
        if (playerScript != null)
        {
            // Define your knockback direction here
            Vector2 knockbackDirection = (player1.transform.position - transform.position).normalized;

            // Trigger knockback on the enemy
            playerScript.TriggerKnockback(knockbackDirection);
        }
    }
}

