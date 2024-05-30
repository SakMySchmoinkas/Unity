using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public float moveSpeed = 7.5f;
    public float baseSpeed = 7.5f;
    public float dashSpeed = 12.5f;
    public float health = 100;
    public float maxHealth = 100;
    public float stamina = 100;
    public float maxStamina = 100f;
    public float staminaDecreaseRate = 10f;
    public float staminaIncreaseRate = 5f;
    public float collisionDamage = 5;
    public float damageAmount = 10f;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    public float knockbackDistance = 2f;
    public LayerMask obstacleLayer;
    private bool isKnockedBack = false;
    public int level = 1;
    public float maxXP = 100f;
    public float currentXP = 0f;
    public float xpMultiplier = 1.2f;
    public bool isSprinting = false;
    public bool canSprint = true;
    public float distanceTravelled = 0f;

    void Update()
    {
        if (health <= 0)
        {
            // Call a method to handle game over
            GameOverLost();
        }
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
        bool isMoving = GetComponent<Rigidbody2D>().velocity.magnitude > 0;

        isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (isSprinting && stamina > 0 && isMoving && canSprint)
        {
            stamina -= staminaDecreaseRate * Time.deltaTime;
        }
        else if (stamina < maxStamina)
        {
            stamina += staminaIncreaseRate * Time.deltaTime;
        }

        stamina = Mathf.Clamp(stamina, 0f, maxStamina);

        if (stamina < 1f)
        {
            canSprint = false;
        }

        if (stamina > 30)
        {
            canSprint = true;
        }

        if (isSprinting && canSprint)
        {
            moveSpeed = dashSpeed;
        }
        else
        {
            moveSpeed = baseSpeed;
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalInput * moveSpeed, verticalInput * moveSpeed);

        // Update distance travelled
        if(isMoving)
        {
        distanceTravelled = distanceTravelled + (Vector2.Distance(Vector2.zero, transform.position)/7000) ;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        CameraShake CameraShakeScript = FindObjectOfType<CameraShake>();
        if (collision.gameObject.tag.Contains("Enemy"))
        {
            CameraShakeScript.timer = CameraShakeScript.shakeDuration;
            CameraShakeScript.isShaking = true;
            

            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;

            StartCoroutine(Knockback(knockbackDirection));
        }
    }

    IEnumerator Knockback(Vector2 knockbackDirection)
    {
        if (!isKnockedBack)
        {

            isKnockedBack = true;

            Vector2 initialPosition = transform.position;
            Vector2 knockbackLocation = initialPosition + knockbackDirection * knockbackDistance;
            health = health - collisionDamage;
            Debug.Log("Player Health: " + health);

            GetComponent<Rigidbody2D>().isKinematic = true;

            float elapsedTime = 0f;

            RaycastHit2D hit = Physics2D.Raycast(initialPosition, knockbackDirection, knockbackDistance + 3f, obstacleLayer);
            if (hit.collider != null)
            {
                knockbackLocation = hit.point - knockbackDirection * 0.1f;
            }

            while (elapsedTime < knockbackDuration)
            {
                transform.position = Vector2.Lerp(initialPosition, knockbackLocation, elapsedTime / knockbackDuration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = knockbackLocation;

            GetComponent<Rigidbody2D>().isKinematic = false;

            isKnockedBack = false;
        }
    }

    public void TriggerKnockback(Vector2 knockbackDirection)
    {
        StartCoroutine(Knockback(knockbackDirection));
    }

    void GameOverLost()
    {
        // Load the "gameOverLost" scene
        SceneManager.LoadScene("gameOverLost");
    }
}
