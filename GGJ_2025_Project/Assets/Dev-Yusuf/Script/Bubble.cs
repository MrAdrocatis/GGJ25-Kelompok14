using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("Bubble Settings")]
    public float initialUpwardForce = 5f;
    public float dragFactor = 0.01f; // Reduced drag for smoother deceleration
    public float selfDestructTime = 10f;
    public float enemyReleaseTime = 5f;
    public float forceDelay = 1.2f; // Waktu jeda untuk gaya awal

    private Rigidbody2D rb;
    public GameObject trappedEnemy { get; private set; }
    private float spawnTime;
    private bool hasReachedBoundary = false;

    public bool IsTrappingEnemy => trappedEnemy != null;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        spawnTime = Time.time;
        Debug.Log($"Bubble spawned. Initial upward force will be applied after {forceDelay} seconds.");
        Invoke(nameof(ApplyInitialForce), forceDelay);
    }

    void Update()
    {
        if (!hasReachedBoundary)
        {
            if (rb.velocity.y > initialUpwardForce)
            {
                rb.velocity -= new Vector2(0, dragFactor * Time.deltaTime);
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, initialUpwardForce));
            }
        }

        if (!IsTrappingEnemy && Time.time - spawnTime > selfDestructTime)
        {
            Debug.Log("Bubble self-destructed after timeout.");
            Destroy(gameObject);
        }
    }

    void ApplyInitialForce()
    {
        rb.velocity += new Vector2(0, initialUpwardForce);
        Debug.Log($"Initial upward force applied: {initialUpwardForce}");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Bubble collided with: {collision.gameObject.name}, Tag: {collision.tag}");

        if (collision.CompareTag("Enemy") && !IsTrappingEnemy)
        {
            Debug.Log("Attempting to trap enemy...");
            TrapEnemy(collision.gameObject);
        }
        else if (collision.CompareTag("Dart") && IsTrappingEnemy)
        {
            Destroy(trappedEnemy);
            Debug.Log("Bubble and trapped enemy destroyed by Dart.");
            Destroy(gameObject);
        }
    }

    private void TrapEnemy(GameObject enemy)
    {
        if (trappedEnemy != null)
        {
            Debug.LogWarning("Bubble already trapping an enemy, ignoring new collision.");
            return;
        }

        trappedEnemy = enemy;
        trappedEnemy.transform.SetParent(transform);
        trappedEnemy.GetComponent<EnemyManager>()?.SetTrapped(true);
        spawnTime = Time.time; // Reset timer
        rb.velocity = Vector2.zero;
        Debug.Log($"Enemy {enemy.name} trapped by bubble.");
        Invoke(nameof(ReleaseEnemy), enemyReleaseTime);
    }

    private void ReleaseEnemy()
    {
        if (IsTrappingEnemy)
        {
            Debug.Log($"Releasing trapped enemy: {trappedEnemy.name}");
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
        Debug.Log("Bubble reached boundary and stopped vertical force.");
    }
}