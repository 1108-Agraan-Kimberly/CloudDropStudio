using UnityEngine;
using System.Collections;

public class newEnemyAtk : MonoBehaviour
{
    public int damage = 1;
    public float knockbackForce = 5f; // Force of the knockback
    public float knockbackCooldown = 2f; // Cooldown duration after knockback

    private bool canAttack = true; // Flag to control attack state

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canAttack) return; // Prevent attacking during cooldown

        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Check if the collided object has a Health component
        Health playerHealth = collision.gameObject.GetComponent<Health>();
        if (playerHealth != null)
        {
            // Apply damage
            playerHealth.healthState(-damage);

            // Apply knockback
            Movement playerMovement = collision.gameObject.GetComponent<Movement>();
            if (playerMovement != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                playerMovement.ApplyKnockback(knockbackDirection * knockbackForce, 0.5f); // Apply knockback for 0.5 seconds
            }

            // Start cooldown
            StartCoroutine(KnockbackCooldown());
        }
    }

    private IEnumerator KnockbackCooldown()
    {
        canAttack = false; // Disable attacking
        yield return new WaitForSeconds(knockbackCooldown); // Wait for cooldown duration
        canAttack = true; // Re-enable attacking
    }
}