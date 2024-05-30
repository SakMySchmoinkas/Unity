using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sleigh : MonoBehaviour
{
    
    public PlayerResources playerResources;
    public bool isInRange = false;
    public List<GameObject> collectedParts = new List<GameObject>();
    public int partsCount = 0;

    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E) && playerResources != null)
        {
            CollectAndStoreSleighPart();   
        }
    }

    void CollectAndStoreSleighPart()
    {
        // Get the first non-empty sleigh part from PlayerResources
        GameObject collectedPart = playerResources.GetAndClearFirstSleighPart();

        if (collectedPart != null)
        {
            partsCount += 1;
            Debug.Log("Sleigh parts added: " + collectedPart); 
            Debug.Log("Collected: " + collectedPart.name);

            // Add the collected sleigh part to the list
            collectedParts.Add(collectedPart);

            if (collectedPart.name.Contains("Body"))
            {
                    A.SetActive(true);
                Debug.Log("Activated: " + A.name);
            }
            
            else if (collectedPart.name.Contains("Leg"))
            {
                B.SetActive(true);
                Debug.Log("Activated: " + B.name);
            }
            
            else if (collectedPart.name.Contains("EngineA"))
            {
                C.SetActive(true);
                Debug.Log("Activated: " + C.name);
            }
            
            else if (collectedPart.name.Contains("EngineB"))
            {
                D.SetActive(true);
                Debug.Log("Activated: " + D.name);
            }
           
        }
        if (partsCount == 4)
        {
            Debug.Log("You win");
            SceneManager.LoadScene("gameOverWon");
        }
        else
        {
            Debug.Log("Current sleigh parts = " + partsCount);
            Debug.Log("Find more Sleigh parts");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Undetectable"))
        {
            playerResources = collision.gameObject.GetComponent<PlayerResources>();
            isInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Undetectable"))
        {
            isInRange = false;
        }
    }
}
