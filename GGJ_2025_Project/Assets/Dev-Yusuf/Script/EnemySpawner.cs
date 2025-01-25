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
    public float intensityIncreasePerMinute = 0.1f; // Intensitas peningkatan per menit (default 0.1)

    private float timeSinceLastSpawn;
    private float currentInterval;

    private void Start()
    {
        currentInterval = spawnInterval;
        InvokeRepeating(nameof(IncreaseSpawnIntensity), 60f, 60f); // Mempercepat interval tiap menit
    }

    private void Update()
    {
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

        // Pilih enemy secara acak berdasarkan probabilitas
        GameObject selectedEnemy = GetRandomEnemy();
        if (selectedEnemy == null) return;

        // Tentukan posisi spawn di dalam area
        float spawnX = Random.Range(bottomLeft.x, topRight.x);
        float spawnY = Random.Range(bottomLeft.y, topRight.y);
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        // Spawn enemy
        Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);
    }

    private GameObject GetRandomEnemy()
    {
        float totalProbability = 0f;
        foreach (var enemy in enemyPrefabs)
        {
            totalProbability += enemy.spawnProbability;
        }

        float randomValue = Random.Range(0f, totalProbability);
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
}