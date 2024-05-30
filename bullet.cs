using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Specify the amount of damage the bullet inflicts
    public float damageAmount;
    public float range = 5;
    public String GameTag = "";
    private Vector2 initialPosition;

    void Start()
    {
        player playerScript = FindObjectOfType<player>();
        damageAmount = playerScript.damageAmount;
        initialPosition = transform.position;
    }

    void Update()
    {
        if (Vector2.Distance(initialPosition, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    // This function is called when the current Collider2D (this bullet) has entered another Collider2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider has the tag "Enemy"
        if (other.tag.Contains(GameTag))
        {
            // Get the enemy's health component
            EnemyStats enemyHealth = other.GetComponent<EnemyStats>();

            // Check if the enemy has a health component
            if (enemyHealth != null)
            {
                // Reduce the enemy's health by the specified amount
                enemyHealth.enemyHealth -= damageAmount;

                // Trigger knockback on the enemy
                TriggerKnockback(other.gameObject);
            }

            // Destroy the bullet game object
            Destroy(gameObject);
        }
    }

    // Trigger knockback on the enemy of type EnemyType1
private void TriggerKnockbackType1(GameObject enemy)
{
    EnemyType1 enemyType1Script = enemy.GetComponent<EnemyType1>();

    if (enemyType1Script != null)
    {
        Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
        enemyType1Script.TriggerKnockback(knockbackDirection);
    }
}

// Trigger knockback on the enemy of type EnemyType5
private void TriggerKnockbackType5(GameObject enemy)
{
    EnemyType5 enemyType5Script = enemy.GetComponent<EnemyType5>();

    if (enemyType5Script != null)
    {
        Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
        enemyType5Script.TriggerKnockback(knockbackDirection);
    }
}

// Main TriggerKnockback method
private void TriggerKnockback(GameObject enemy)
{
    // Try to get the EnemyType1 script
    TriggerKnockbackType1(enemy);

    // If not found, try to get the EnemyType5 script
    if (!enemy.GetComponent<EnemyType1>())
    {
        TriggerKnockbackType5(enemy);
    }
}



}
