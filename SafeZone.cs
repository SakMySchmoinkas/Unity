using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public string undetectableTag = "Undetectable";
    public string defaultTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(defaultTag))
        {
            // Change the player's tag to undetectable
            other.tag = undetectableTag;
        }

        if (other.tag.Contains("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(undetectableTag))
        {
            // Change the player's tag back to the default tag
            other.tag = defaultTag;
        }
    }
}
