using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("Bubble Settings")]
    public float initialUpwardForce = 5f;
    public float selfDestructTime = 10f;
    public float enemyReleaseTime = 5f;

    private Rigidbody2D rb;
    private GameObject trappedEnemy;
    private float spawnTime;
    private bool hasReachedBoundary = false;

    public bool IsTrappingEnemy => trappedEnemy != null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, initialUpwardForce);
        spawnTime = Time.time;
    }

    void Update()
    {
        if (!hasReachedBoundary && rb.velocity.y < initialUpwardForce)
        {
            rb.velocity = new Vector2(rb.velocity.x, initialUpwardForce);
        }

        if (!IsTrappingEnemy && Time.time - spawnTime > selfDestructTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !IsTrappingEnemy)
        {
            TrapEnemy(collision.gameObject);
        }
        else if (collision.CompareTag("Dart") && IsTrappingEnemy)
        {
            Destroy(trappedEnemy);
            Destroy(gameObject);
        }
    }

    private void TrapEnemy(GameObject enemy)
    {
        trappedEnemy = enemy;
        trappedEnemy.transform.SetParent(transform);
        trappedEnemy.GetComponent<EnemyManager>()?.SetTrapped(true);
        spawnTime = Time.time; // Reset timer
        rb.velocity = Vector2.zero;
        Invoke(nameof(ReleaseEnemy), enemyReleaseTime);
    }

    private void ReleaseEnemy()
    {
        if (IsTrappingEnemy)
        {
            trappedEnemy.GetComponent<EnemyManager>()?.SetTrapped(false);
            trappedEnemy.transform.SetParent(null);
            trappedEnemy = null;
            Destroy(gameObject);
        }
    }

    public void StopVerticalForce()
    {
        hasReachedBoundary = true;
        rb.velocity = Vector2.zero;
    }
}