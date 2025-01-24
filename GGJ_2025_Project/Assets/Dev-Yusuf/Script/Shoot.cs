using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Input Settings")]
    public KeyCode shootKey = KeyCode.Space; // Tombol untuk menembak

    [Header("Projectile Settings")]
    public GameObject projectilePrefab; // Prefab projectile
    public GameObject firePointVFX; // VFX yang ditampilkan di firePoint
    public float firePointOffset = 0.5f; // Jarak firePoint dari pemain
    public float projectileSpeed = 10f; // Kecepatan projectile
    public float shootCooldown = 0.5f; // Jeda antara tembakan (dalam detik)

    [Header("Sprites for Aiming")]
    public Sprite idleSprite; // Sprite default saat idle
    public Sprite upSprite; // Sprite untuk arah atas
    public Sprite leftSprite; // Sprite untuk arah kiri
    public Sprite rightSprite; // Sprite untuk arah kanan
    public Sprite upLeftSprite; // Sprite untuk arah diagonal atas-kiri
    public Sprite upRightSprite; // Sprite untuk arah diagonal atas-kanan

    private SpriteRenderer spriteRenderer;
    private Vector2 shootDirection;
    private float lastShootTime; // Waktu terakhir menembak

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        shootDirection = Vector2.zero; // Arah awal kosong
        lastShootTime = -shootCooldown; // Agar bisa langsung menembak di awal
    }

    void Update()
    {
        HandleInput();
        HandleShooting();
    }

    void HandleInput()
    {
        // Reset arah tembak
        shootDirection = Vector2.zero;

        // Deteksi input arah
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) shootDirection += Vector2.up;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) shootDirection += Vector2.left;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) shootDirection += Vector2.right;

        // Normalisasi arah agar tetap konsisten
        if (shootDirection != Vector2.zero) shootDirection.Normalize();

        // Update sprite berdasarkan arah atau idle
        UpdateSprite();
    }

    void HandleShooting()
    {
        // Cek apakah tombol menembak ditekan dan jeda tembakan terpenuhi
        if (Input.GetKeyDown(shootKey) && Time.time >= lastShootTime + shootCooldown && shootDirection != Vector2.zero)
        {
            ShootProjectile();
            lastShootTime = Time.time; // Perbarui waktu terakhir menembak
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null)
        {
            // Hitung posisi firePoint
            Vector3 firePointPosition = transform.position + (Vector3)(shootDirection * firePointOffset);

            // Buat projectile di firePoint dengan arah yang sesuai
            GameObject projectile = Instantiate(projectilePrefab, firePointPosition, Quaternion.identity);

            // Tambahkan velocity ke projectile
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = shootDirection * projectileSpeed; // Atur kecepatan projectile
            }

            // Tampilkan VFX di firePoint
            if (firePointVFX != null)
            {
                Instantiate(firePointVFX, firePointPosition, Quaternion.identity);
            }
        }
    }

    void UpdateSprite()
    {
        // Ganti sprite berdasarkan arah tembak
        if (shootDirection == Vector2.up)
        {
            spriteRenderer.sprite = upSprite;
        }
        else if (shootDirection == Vector2.left)
        {
            spriteRenderer.sprite = leftSprite;
        }
        else if (shootDirection == Vector2.right)
        {
            spriteRenderer.sprite = rightSprite;
        }
        else if (shootDirection == (Vector2.up + Vector2.left).normalized)
        {
            spriteRenderer.sprite = upLeftSprite;
        }
        else if (shootDirection == (Vector2.up + Vector2.right).normalized)
        {
            spriteRenderer.sprite = upRightSprite;
        }
        else
        {
            spriteRenderer.sprite = idleSprite; // Kembali ke idle sprite
        }
    }
}