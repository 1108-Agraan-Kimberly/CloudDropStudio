using UnityEngine;
using System.Collections;

public class player_atk : MonoBehaviour
{
    public Animator animator;
    public float cooldown = 0.5f;
    private float timer;
    public Transform range;
    public float attackRange = 2f;
    public LayerMask enemyLayer;
    public int damage = 1;

    public GameObject projectilePrefab; // Reference to the projectile prefab
    public float projectileSpeed = 10f; // Speed of the projectile

    private Movement movementScript;

    private void Start()
    {
        if (projectilePrefab != null)
        {
            Debug.Log("Projectile prefab is assigned: " + projectilePrefab.name);
        }
        else
        {
            Debug.LogError("Projectile prefab is not assigned.");
        }
        movementScript = GetComponent<Movement>(); // Reference the Movement script
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        // Check for right mouse button click to shoot
        if (Input.GetMouseButtonDown(1))
        {
            Shoot();
        }
    }

    public void Attack()
    {
        if (timer <= 0)
        {
            animator.SetBool("isAttacking", true);

            // Update the range position based on the player's facing direction
            Vector2 attackDirection = movementScript.LastFacingDirection;
            if (attackDirection != Vector2.zero) // Ensure a valid direction
            {
                range.localPosition = attackDirection * attackRange;
            }

            // Detect enemies in the attack range
            Collider2D[] enemies = Physics2D.OverlapCircleAll(range.position, attackRange, enemyLayer);
            foreach (Collider2D enemy in enemies)
            {
                enemy_health enemyHealth = enemy.GetComponent<enemy_health>();
                if (enemyHealth != null)
                {
                    enemyHealth.ChangeHealth(-damage); // Apply damage to the enemy
                }
            }

            timer = cooldown;
        }
    }

    public void Shoot()
    {
        if (timer <= 0)
        {
            // Get the mouse position in world space
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure the z-coordinate is 0

            // Calculate the direction from the player to the mouse position
            Vector2 shootDirection = (mousePosition - transform.position).normalized;

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Set the projectile's direction and speed
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetDirection(shootDirection, projectileSpeed);
            }

            timer = cooldown; // Reset the cooldown timer
        }
    }

    public void FinishAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the attack range in the Scene view
        if (range != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(range.position, attackRange);
        }
    }
}



