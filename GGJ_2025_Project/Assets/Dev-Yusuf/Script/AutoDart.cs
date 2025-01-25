using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDart : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab; // The projectile to spawn
    public float bulletVelocity = 5f; // Speed of the bullet
    public float fireInterval = 1f; // Time between each shot
    public Vector2 firePointOffset = new Vector2(0, 1f); // Offset from the center to spawn bullets

    private void Start()
    {
        // Start the automatic shooting coroutine
        StartCoroutine(ShootProjectileRoutine());
    }

    private IEnumerator ShootProjectileRoutine()
    {
        while (true)
        {
            ShootProjectile();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null)
        {
            UnityEngine.Debug.Log("Projectile Prefab is not assigned.");
            return;
        }

        // Calculate the spawn position
        Vector2 spawnPosition = (Vector2)transform.position + firePointOffset;

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Set the velocity of the projectile to move upward
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.up * bulletVelocity;
        }
        else
        {
            UnityEngine.Debug.Log("Projectile Prefab does not have a Rigidbody2D component.");
        }
    }
}