using System.Collections;
using UnityEngine;

public class BoostLogic : MonoBehaviour
{
    public float boostDuration = 5f;
    public float boostAmount = 10f; // Adjust the boost amount as needed
    public float destroyTime = 5f;
    public bool isBoosted = false;
    public float currentTime=0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isBoosted && (other.CompareTag("Player") || other.CompareTag("Undetectable")))
        {
            StartCoroutine(ApplyBoost(other.gameObject));
        }
    }

    void Update(){
        
        currentTime+=Time.deltaTime;

        if (isBoosted==false && currentTime>=destroyTime){
            Destroy(gameObject);
        }
        
    }

    IEnumerator ApplyBoost(GameObject player)
{
    isBoosted = true;

    shooting shootingScript = player.GetComponent<shooting>();
    player playerScript = player.GetComponent<player>();

    if (shootingScript != null && playerScript != null)
    {
        // Store the original values before applying the boost
        float originalProjectileSpeed = shootingScript.projectileSpeed;
        float originalDashSpeed = playerScript.dashSpeed;
        float originalMoveSpeed = playerScript.moveSpeed;
        float originalBaseSpeed = playerScript.baseSpeed;

        // Calculate the boosted values, capped at the specified maximums
        float boostedProjectileSpeed = Mathf.Min(originalProjectileSpeed + boostAmount, 15f);
        float boostedDashSpeed = Mathf.Min(originalDashSpeed + boostAmount, 21f);
        float boostedMoveSpeed = Mathf.Min(originalMoveSpeed + boostAmount, 15f);
        float boostedBaseSpeed = Mathf.Min(originalBaseSpeed + boostAmount, 15f);

        // Apply the boost effect
        shootingScript.projectileSpeed = boostedProjectileSpeed;
        playerScript.dashSpeed = boostedDashSpeed;
        playerScript.moveSpeed = boostedMoveSpeed;
        playerScript.baseSpeed = boostedBaseSpeed;

        // Disable the visual representation of the boost (optional)
        GetComponent<SpriteRenderer>().enabled = false;

        // Wait for the boost duration
        yield return new WaitForSeconds(boostDuration);

        // Revert the changes after the boost duration
        shootingScript.projectileSpeed = originalProjectileSpeed;
        playerScript.dashSpeed = originalDashSpeed;
        playerScript.moveSpeed = originalMoveSpeed;
        playerScript.baseSpeed = originalBaseSpeed;

        // Destroy the boost GameObject after reverting the changes
        Destroy(gameObject);
    }
    else
    {
        if (shootingScript == null)
            Debug.LogError("shooting script not found on the player.");
        
        if (playerScript == null)
            Debug.LogError("player script not found on the player.");
    }
}



    void DestroyObject()
    {
        // Destroy the GameObject this script is attached to
        Destroy(gameObject);
    }
}