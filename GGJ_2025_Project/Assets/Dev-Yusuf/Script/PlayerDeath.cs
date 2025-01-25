using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager

    private void Start()
    {
        // Ensure GameManager is assigned
        if (gameManager == null)
        {
            UnityEngine.Debug.LogError("GameManager is not assigned in PlayerDeath script.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            UnityEngine.Debug.Log("Player collided with Enemy and will now be destroyed.");

            // Notify GameManager about the player's death
            if (gameManager != null)
            {
                gameManager.OnPlayerDeath();
            }

            // Destroy the player game object
            Destroy(gameObject);
        }
    }
}
