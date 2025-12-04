using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    public float detectionRange = 8f;
    public float fireCooldown = 1.5f;
    private float fireTimer = 0f;

    public Transform firePoint;              
    public GameObject projectilePrefab;      

    private Transform player;
    private bool playerInRange = false;

    private void Update()
    {
        
        if (fireTimer > 0)
            fireTimer -= Time.deltaTime;

        
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
            return;
        }

        
        float distance = Vector2.Distance(transform.position, player.position);
        playerInRange = distance <= detectionRange;

        if (playerInRange)
        {
            ShootAtPlayer();
        }
    }

    private void ShootAtPlayer()
    {
        if (fireTimer > 0) return;

        
        Vector2 direction = (player.position - firePoint.position).normalized;

        
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        
        RaccoonEnemyProjectile proj = bullet.GetComponent<RaccoonEnemyProjectile>();
        proj.SetDirection(direction);

        fireTimer = fireCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
