using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    private MonoBehaviour enemyTypeComponent;

    private void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(gameObject.tag))
        {
            // Ignore collisions with objects of the same tag
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, true);
        }
    }
}

    
