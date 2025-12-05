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
    //private spellbook book;

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

            // Detect enemies in the attack range using layer mask
            Collider2D[] enemies = Physics2D.OverlapCircleAll(range.position, attackRange, enemyLayer);

            // Fallback: if no results from layer mask, check all colliders and filter by enemy components/tags
            if (enemies == null || enemies.Length == 0)
            {
                enemies = Physics2D.OverlapCircleAll(range.position, attackRange);
            }

            foreach (Collider2D enemy in enemies)
            {
                if (enemy == null) continue;

                // Ignore the player's own collider(s)
                if (enemy.gameObject == gameObject) continue;
                if (enemy.CompareTag("Player")) continue;

                // Prefer enemy_health component (on collider, parent, or children)
                enemy_health enemyHealth = enemy.GetComponent<enemy_health>();
                if (enemyHealth == null) enemyHealth = enemy.GetComponentInParent<enemy_health>();
                if (enemyHealth == null) enemyHealth = enemy.GetComponentInChildren<enemy_health>();

                if (enemyHealth != null)
                {
                    enemyHealth.ChangeHealth(-damage); // Apply damage to the enemy
                    continue;
                }

                // Also support enemies that use a different health script (robust fallback)
                Health generic = enemy.GetComponent<Health>();
                if (generic == null) generic = enemy.GetComponentInParent<Health>();
                if (generic == null) generic = enemy.GetComponentInChildren<Health>();
                if (generic != null)
                {
                    generic.healthState(-damage);
                    continue;
                }

                // Last resort: if collider is tagged as Enemy, try to damage a known component on the same GameObject
                if (enemy.CompareTag("Enemy"))
                {
                    // try enemy_health again on the root
                    GameObject go = enemy.gameObject;
                    enemyHealth = go.GetComponent<enemy_health>();
                    if (enemyHealth == null) enemyHealth = go.GetComponentInChildren<enemy_health>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.ChangeHealth(-damage);
                    }
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



