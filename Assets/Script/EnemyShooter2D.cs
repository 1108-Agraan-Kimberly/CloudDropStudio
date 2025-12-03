using UnityEngine;

public class EnemyShooter2D : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    public float shootingRange = 10f;
    public float fireRate = 1.5f;
    public float projectileSpeed = 10f;

    private Transform player;
    private bool playerInRange = false;
    private float nextFireTime = 0f;

    private void FixedUpdate()
    {
        if (!playerInRange || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= shootingRange && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        Vector2 dir = (player.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}