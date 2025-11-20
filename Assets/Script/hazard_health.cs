using UnityEngine;

public class hazard_health : MonoBehaviour
{
    public int currentHealth = 10; // Health of the hazard
    public int maxHealth = 10;

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is tagged as "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hazard collided with the player.");

            // Check if the player has a damage-dealing script (e.g., player_atk)
            player_atk playerAttack = collision.gameObject.GetComponent<player_atk>();
            if (playerAttack != null)
            {
                TakeDamage(playerAttack.damage); // Take damage from the player
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("Hazard took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Destroy the hazard if health reaches 0
        }
    }
}