using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("Bubble Settings")]
    public float initialUpwardForce = 5f; // Initial force upwards
    public float airDrag = 0.1f; // Air drag deceleration
    public float randomForceMagnitude = 3f; // Force applied randomly to sides
    public float selfDestructTime = 10f; // Time to self-destruct if no enemy trapped
    public float enemyReleaseTime = 5f; // Time to release enemy if not interacted

    private Rigidbody2D rb;
    private GameObject trappedEnemy;
    private float trapStartTime;

    public bool IsTrappingEnemy { get; private set; } = false; // Public property for external access

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * initialUpwardForce, ForceMode2D.Impulse);
        StartCoroutine(ApplyRandomSideForce());
        Invoke(nameof(SelfDestruct), selfDestructTime);
    }

    void Update()
    {
        if (!IsTrappingEnemy)
        {
            // Apply air drag
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(0, rb.velocity.y - airDrag * Time.deltaTime));
        }
    }

    private IEnumerator ApplyRandomSideForce()
    {
        while (!IsTrappingEnemy)
        {
            yield return new WaitForSeconds(1f);
            float randomDirection = Random.Range(-1f, 1f);
            rb.AddForce(Vector2.right * randomDirection * randomForceMagnitude, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !IsTrappingEnemy)
        {
            TrapEnemy(collision.gameObject);
        }
        else if (collision.CompareTag("SpecialProjectile") && IsTrappingEnemy)
        {
            Destroy(trappedEnemy);
            Destroy(gameObject);
        }
    }

    private void TrapEnemy(GameObject enemy)
    {
        trappedEnemy = enemy;
        IsTrappingEnemy = true;
        trapStartTime = Time.time;
        enemy.GetComponent<EnemyManager>()?.SetTrapped(true);
        rb.velocity = Vector2.zero; // Stop vertical movement
        CancelInvoke(nameof(SelfDestruct)); // Cancel default self-destruction
        Invoke(nameof(ReleaseEnemy), enemyReleaseTime);
    }

    private void ReleaseEnemy()
    {
        if (IsTrappingEnemy && trappedEnemy != null)
        {
            trappedEnemy.GetComponent<EnemyManager>()?.SetTrapped(false);
            trappedEnemy = null;
            IsTrappingEnemy = false;
            Destroy(gameObject);
        }
    }

    private void SelfDestruct()
    {
        if (!IsTrappingEnemy)
        {
            Destroy(gameObject);
        }
    }
}
