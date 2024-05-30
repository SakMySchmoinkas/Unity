using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public GameObject[] SleighParts; // Array of GameObjects for SleighParts

    // Start is called before the first frame update
    void Start()
    {
        SleighParts = new GameObject[4];
    }

    // Method to add a SleighPart to the array
    public void AddSleighPart(GameObject sleighPart)
    {
        // Find the first empty slot in the array
        for (int i = 0; i < SleighParts.Length; i++)
        {
            if (SleighParts[i] == null)
            {
                // Add the GameObject to the array
                SleighParts[i] = sleighPart;
                return; // Exit the loop once added
            }
        }
    }

    public GameObject GetAndClearFirstSleighPart()
    {
        // Iterate through the SleighParts array
        for (int i = 0; i < SleighParts.Length; i++)
        {
            if (SleighParts[i] != null)
            {
                GameObject sleighPart = SleighParts[i];
                SleighParts[i] = null; // Clear the slot
                return sleighPart; // Return the first non-empty SleighPart
            }
        }

        return null; // Return null if no non-empty SleighPart found
    }
}
