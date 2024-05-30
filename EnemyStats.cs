using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float enemyHealth = 50f;
    public int ItemIndex = 0;
    public float offset = 1.0f;
    public float xpAmount = 0f;
    public List<GameObject> gameObjectsList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        ItemIndex = Random.Range(0, gameObjectsList.Count);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyHealth <= 0 && gameObjectsList.Count>0)
        {

            // Instantiate and destroy as before
            Instantiate(gameObjectsList[ItemIndex], (Vector2)transform.position, Quaternion.identity);
            LevelUp();
        }
    }

    void LevelUp()
    {
            CameraShake CameraShakeScript = FindObjectOfType<CameraShake>();
            player playerScript = FindObjectOfType<player>();
            bullet bulletScript = FindObjectOfType<bullet>();
            Spawn spawnScript = FindObjectOfType<Spawn>();
        if (enemyHealth <= 0 && playerScript != null)
        {
            CameraShakeScript.timer = CameraShakeScript.shakeDuration;
            CameraShakeScript.isShaking=true;
            playerScript.currentXP = playerScript.currentXP+xpAmount;
            Destroy(gameObject);
        }
        if(playerScript.currentXP>=playerScript.maxXP){

            playerScript.level++;
            playerScript.currentXP = 0;
            playerScript.maxXP *= playerScript.xpMultiplier;
            playerScript.maxHealth *= 1.5f;
            playerScript.damageAmount *= 1.5f;
            playerScript.maxStamina *= 1.5f;
            playerScript.health = playerScript.maxHealth;
            playerScript.stamina = playerScript.maxStamina;

        }
        if(playerScript.level % 2 == 0 && spawnScript.minSpawnInterval>1.5f)
            {
                spawnScript.minSpawnInterval = spawnScript.minSpawnInterval*0.8f;
                spawnScript.maxSpawnInterval = spawnScript.maxSpawnInterval*0.8f;
            }
    }
}
