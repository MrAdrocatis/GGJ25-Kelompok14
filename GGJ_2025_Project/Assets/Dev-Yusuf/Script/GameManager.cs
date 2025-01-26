// GameManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum WinConditionType
    {
        KillCount,
        Survival
    }

    [Header("Win Condition Settings")]
    public WinConditionType winConditionType; // Select the win condition type
    public int killQuota; // For KillCount
    public float survivalTime; // For Survival mode

    [Header("UI Elements")]
    public TextMeshProUGUI killCountText; // TMP for kill count
    public TextMeshProUGUI countdownText; // TMP for survival countdown
    public GameObject winUI;
    public GameObject loseUI;

    [Header("Audio")]
    public AudioSource bgm;
    
    public AudioSource winSound;
    public AudioSource loseSound;

    [Header("Other Settings")]
    public GameObject player;

    private int currentKills = 0; // Track the number of kills
    private float survivalTimer; // Timer for survival mode
    private bool isGameOver = false;

    private float previousTimeScale = 1f; // Store previous time scale to revert later
    private bool isPaused = false; // Whether the game is paused or not

    [Header("UI Elements")]
    public GameObject pauseMenuUI; // UI to show when the game is paused (optional)
    public GameObject objectToToggle; // GameObject to activate/deactivate on pause

    private void Start()
    {
        survivalTimer = survivalTime; // Initialize survival timer
        UpdateKillCountUI();
        if (countdownText != null)
        {
            countdownText.text = Mathf.Ceil(survivalTimer).ToString();
        }
        if (winUI != null) winUI.SetActive(false);
        if (loseUI != null) loseUI.SetActive(false);
    }

    private void Update()
    {
        if (isGameOver) return;

        if (winConditionType == WinConditionType.Survival)
        {
            survivalTimer -= Time.deltaTime;
            if (countdownText != null)
            {
                countdownText.text = Mathf.Ceil(survivalTimer).ToString();
            }

            if (survivalTimer <= 0)
            {
                TriggerWin();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // Example: using Escape key for pause
        {
            TogglePause();
        }
    }

    public void OnEnemyDefeated()
    {
        if (isGameOver) return;

        currentKills++;
        UpdateKillCountUI();

        if (winConditionType == WinConditionType.KillCount && currentKills >= killQuota)
        {
            TriggerWin();
        }
    }

    private void UpdateKillCountUI()
    {
        if (killCountText != null)
        {
            killCountText.text = $"Kills: {currentKills}";
        }
    }

    public void OnPlayerDeath()
    {
        if (!isGameOver)
        {
            isGameOver = true;

            if (loseSound != null) loseSound.Play();
            if (loseUI != null) loseUI.SetActive(true);
        }
    }

    private void TriggerWin()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            if (winSound != null) winSound.Play();
            if (winUI != null) winUI.SetActive(true);
        }
    }

 

    // Function to toggle pause and resume
    public void TogglePause()
    {
        if (isPaused)
        {
            // If the game is already paused, resume it by reverting Time.timeScale
            Time.timeScale = previousTimeScale; // Revert to the previous time scale
            isPaused = false; // Mark as not paused

            // Deactivate the pause menu and the object
            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(false); // Hide the pause menu
            }
            if (objectToToggle != null)
            {
                objectToToggle.SetActive(false); // Deactivate the object
            }
        }
        else
        {
            // If the game is not paused, pause it
            previousTimeScale = Time.timeScale; // Store the current time scale
            Time.timeScale = 0f; // Stop time (pause the game)
            isPaused = true; // Mark as paused

            // Activate the pause menu and the object
            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(true); // Show the pause menu
            }
            if (objectToToggle != null)
            {
                objectToToggle.SetActive(true); // Activate the object
            }
        }
    }
}

