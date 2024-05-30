using System.Collections;
using UnityEngine;

public class ShieldLogic : MonoBehaviour
{
    public float ShieldDuration = 5f;
    private bool isShielded = false;
    public float destroyTime = 5f;
    public float currentTime=0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isShielded && (other.CompareTag("Player") || other.CompareTag("Undetectable")))
        {
            StartCoroutine(ApplyShield(other.gameObject));
        }
    }

    void Update(){
        
        currentTime+=Time.deltaTime;

        if (isShielded==false && currentTime>=destroyTime){
            Destroy(gameObject);
        }
        
    }

    IEnumerator ApplyShield(GameObject player)
    {
        isShielded = true;

        player playerScript = player.GetComponent<player>();

        if (playerScript != null)
        {
            // Apply the boost effect
            playerScript.collisionDamage=0;

            // Disable the visual representation of the boost (optional)
            GetComponent<SpriteRenderer>().enabled = false;

            // Wait for the boost duration
            yield return new WaitForSeconds(ShieldDuration);

            // Revert the changes after the boost duration
            playerScript.collisionDamage=5;

            // Destroy the boost GameObject after reverting the changes
            Destroy(gameObject);
        }
    }

    void DestroyObject()
    {
        // Destroy the GameObject this script is attached to
        Destroy(gameObject);
    }
}
