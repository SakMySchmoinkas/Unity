using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    public int bulletCount = 30;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player playerScript = FindObjectOfType<player>();

        if (Input.GetMouseButtonDown(0) && player != null && bulletCount > 0)
        {
            // Get the mouse position in world space
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            // Calculate the direction towards the mouse cursor
            Vector2 direction = (mousePosition - transform.position).normalized;

            // Instantiate a projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Set the projectile's velocity
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = direction * projectileSpeed;
            bulletCount--;
        }

        // Check if the distance travelled is approximately a multiple of 5
        if (playerScript.distanceTravelled > 5)
        {
            bulletCount += 3;
            playerScript.distanceTravelled=0;
        }
    }
}
