using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
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


                animator.SetTrigger("IsLose");

                // Destroy the player game object
                Destroy(gameObject, 5f);
            }
        }
    }
}
