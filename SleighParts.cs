using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleighParts : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Undetectable"))
        {
            PlayerResources resourcesScript = other.GetComponent<PlayerResources>();

            if (resourcesScript != null)
            {
                // Add the collected GameObject to the SleighParts array
                resourcesScript.AddSleighPart(gameObject);

                // Disable the sleigh part GameObject after it is collected
                gameObject.SetActive(false);
            }
        }
    }
}

