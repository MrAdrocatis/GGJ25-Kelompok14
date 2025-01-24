using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Settings")]
    public Transform player; // Objek pemain yang akan dikejar
    public float moveSpeed = 2f; // Kecepatan pergerakan enemy
    public float randomFactor = 0.5f; // Tingkat variasi acak pada arah gerakan
    public float randomInterval = 0.5f; // Interval waktu untuk memperbarui arah acak

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 randomOffset;
    private float timeSinceLastRandomUpdate;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        randomOffset = Vector2.zero;
        timeSinceLastRandomUpdate = 0f;
    }

    void Update()
    {
        if (player != null) // Pastikan pemain sudah diassign
        {
            // Hitung arah menuju pemain
            Vector2 direction = (player.position - transform.position).normalized;

            // Update randomOffset secara berkala
            timeSinceLastRandomUpdate += Time.deltaTime;
            if (timeSinceLastRandomUpdate >= randomInterval)
            {
                // Tambahkan variasi acak pada arah
                randomOffset = new Vector2(
                    Random.Range(-randomFactor, randomFactor),
                    Random.Range(-randomFactor, randomFactor)
                );
                timeSinceLastRandomUpdate = 0f;
            }

            // Gabungkan arah utama dengan offset acak
            movement = (direction + randomOffset).normalized;
        }
    }

    void FixedUpdate()
    {
        // Gerakkan enemy menuju pemain
        MoveEnemy();
    }

    void MoveEnemy()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
