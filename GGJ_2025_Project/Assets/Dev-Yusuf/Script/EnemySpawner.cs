using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyPrefab
    {
        public GameObject prefab; // Prefab enemy
        [Range(0f, 1f)] public float spawnProbability; // Probabilitas spawn
    }

    [Header("Spawn Settings")]
    public List<EnemyPrefab> enemyPrefabs; // Daftar prefab enemy dengan probabilitas
    public Vector2 bottomLeft; // Koordinat bawah kiri area spawn
    public Vector2 topRight; // Koordinat atas kanan area spawn
    public float spawnInterval = 2f; // Jeda antar spawn
    public float intensityIncreasePerMinute = 0.1f; // Intensitas peningkatan per menit
    public int maxSpawnCount = 0; // Jumlah maksimal musuh yang boleh di-spawn (0 = infinite)

    private float timeSinceLastSpawn;
    private float currentInterval;
    private int totalSpawnedEnemies = 0; // Jumlah total musuh yang di-spawn
    private GameManager gameManager; // Referensi ke GameManager
    private bool spawningEnabled = true; // Status apakah spawner masih aktif

    private void Start()
    {
        currentInterval = spawnInterval;
        gameManager = FindObjectOfType<GameManager>(); // Cari GameManager di scene
        InvokeRepeating(nameof(IncreaseSpawnIntensity), 60f, 60f); // Mempercepat interval tiap menit
    }

    private void Update()
    {
        if (!spawningEnabled) return;

        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= currentInterval)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0) return;

        GameObject selectedEnemy = GetRandomEnemy();
        if (selectedEnemy == null) return;

        float spawnX = UnityEngine.Random.Range(bottomLeft.x, topRight.x);
        float spawnY = UnityEngine.Random.Range(bottomLeft.y, topRight.y);
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);

        totalSpawnedEnemies++;
        if (maxSpawnCount > 0 && totalSpawnedEnemies >= maxSpawnCount)
        {
            spawningEnabled = false;
            CancelInvoke(nameof(IncreaseSpawnIntensity));
        }
    }

    private GameObject GetRandomEnemy()
    {
        float totalProbability = 0f;
        foreach (var enemy in enemyPrefabs)
        {
            totalProbability += enemy.spawnProbability;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;

        foreach (var enemy in enemyPrefabs)
        {
            cumulativeProbability += enemy.spawnProbability;
            if (randomValue <= cumulativeProbability)
            {
                return enemy.prefab;
            }
        }

        return null;
    }

    private void IncreaseSpawnIntensity()
    {
        currentInterval = Mathf.Max(0.1f, currentInterval - intensityIncreasePerMinute); // Minimal 0.1 detik
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(bottomLeft.x, bottomLeft.y, 0), new Vector3(bottomLeft.x, topRight.y, 0));
        Gizmos.DrawLine(new Vector3(bottomLeft.x, topRight.y, 0), new Vector3(topRight.x, topRight.y, 0));
        Gizmos.DrawLine(new Vector3(topRight.x, topRight.y, 0), new Vector3(topRight.x, bottomLeft.y, 0));
        Gizmos.DrawLine(new Vector3(topRight.x, bottomLeft.y, 0), new Vector3(bottomLeft.x, bottomLeft.y, 0));
    }

    public bool IsSpawnerFinished()
    {
        return !spawningEnabled; // Mengembalikan status apakah spawner selesai
    }
}
