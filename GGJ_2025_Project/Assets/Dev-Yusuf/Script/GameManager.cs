using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Public fields to assign in the Unity Inspector
    public int enemyCount;
    public GameObject winUI;
    public GameObject loseUI;
    public AudioSource winSound;
    public AudioSource loseSound;
    public GameObject player; // Manually assign the Player object in the Inspector

    // Private field to track if the game is over
    private bool isGameOver = false;

    void Start()
    {
        // Ensure all UI elements are inactive at the start
        if (winUI != null) winUI.SetActive(false);
        if (loseUI != null) loseUI.SetActive(false);
    }

    // Method to handle player death
    public void OnPlayerDeath()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            UnityEngine.Debug.Log("You Lose.");
            if (player != null) Destroy(player); // Destroy the player game object

            // Play lose sound and activate lose UI
            if (loseSound != null) loseSound.Play();
            if (loseUI != null) loseUI.SetActive(true);
        }
    }

    // Method to check win condition
    public void OnEnemyDefeated()
    {
        if (!isGameOver)
        {
            enemyCount--;
            if (enemyCount <= 0)
            {
                isGameOver = true;
                UnityEngine.Debug.Log("You Win.");
                // Play win sound and activate win UI
                if (winSound != null) winSound.Play();
                if (winUI != null) winUI.SetActive(true);
            }
        }
    }

    // Example of collision handling
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.CompareTag("Player"))
        {
            OnPlayerDeath(); // Handle player death logic
        }
    }
}


