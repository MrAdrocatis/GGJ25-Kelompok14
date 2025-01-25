using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [Header("Dart Settings")]
    public float selfDestructTime = 10f; // Waktu sebelum dart hancur otomatis
    public float speed = 15f; // Kecepatan dart

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Memberikan kecepatan awal pada dart
        rb.velocity = Vector2.up * speed;

        // Mengatur penghancuran otomatis setelah waktu tertentu
        Destroy(gameObject, selfDestructTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Dart collided with: {collision.gameObject.name}, Tag: {collision.tag}");

        if (collision.CompareTag("Bubble"))
        {
            Bubble bubble = collision.GetComponent<Bubble>();

            // Hanya hancurkan bubble yang sedang menangkap musuh
            if (bubble != null && bubble.IsTrappingEnemy)
            {
                // Hancurkan musuh dan bubble
                Destroy(bubble.trappedEnemy);
                Destroy(bubble.gameObject);
                Debug.Log("Dart destroyed a bubble with trapped enemy.");
            }
            else
            {
                // Tidak menghancurkan bubble kosong
                Debug.Log("Dart hit an empty bubble but did not destroy it.");
            }

            // Hancurkan dart setelah trigger
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Block"))
        {
            // Hancurkan dart jika bertabrakan dengan block
            Debug.Log("Dart collided with a block and is destroyed.");
            Destroy(gameObject);
        }
    }
}


