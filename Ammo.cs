using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public float destroyTime = 5f;

    void Start()
    {
        // Invoke the DestroyObject method after 'destroyTime' seconds
        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Undetectable"))
        {
            shooting shootingScript = other.GetComponent<shooting>();

            if (shootingScript != null)
            {
                // Increment the gold count variable
                shootingScript.bulletCount+=10;



                // Destroy the coin GameObject after it is collected
                Destroy(gameObject);
            }
        }
    }
    void DestroyObject()
    {
        // Destroy the GameObject this script is attached to
        Destroy(gameObject);
    }
}