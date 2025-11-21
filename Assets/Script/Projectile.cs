using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the projectile
    public float lifetime = 5f; // Time before the projectile is destroyed

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true; // Ensure the Rigidbody2D is kinematic to avoid physics interactions
        }
    }

    private void Start()
    {
        // Destroy the projectile after a longer lifetime to ensure it travels across the screen
        Destroy(gameObject, lifetime);

        // Ensure the projectile is on the correct physics layer to avoid collisions with the player
        gameObject.layer = LayerMask.NameToLayer("Projectile");
    }

    public void SetDirection(Vector2 direction, float speed)
    {
        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * speed; // Set the velocity for straight-line movement
            rb.isKinematic = true; // Ensure the Rigidbody2D is kinematic to avoid physics interactions
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collision with the player
        if (collision.CompareTag("Player")) return;

        // Check if the projectile hits an enemy
        enemy_health enemyHealth = collision.GetComponent<enemy_health>();
        if (enemyHealth != null)
        {
            enemyHealth.ChangeHealth(-damage); // Deal damage to the enemy
            Destroy(gameObject); // Destroy the projectile
        }

        // Handle other collisions (e.g., walls)
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject); // Destroy the projectile on wall collision
        }
    }

    private void OnBecameInvisible()
    {
        // Destroy the projectile if it leaves the camera view
        Destroy(gameObject);
    }
}