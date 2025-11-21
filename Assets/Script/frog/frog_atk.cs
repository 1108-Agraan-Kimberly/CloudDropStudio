using UnityEngine;
using System.Collections;

public class frog_atk : MonoBehaviour
{
    public int damage = 1;
    public float stunDuration = 1f;

    private bool isPlayerStunned = false;
    private Movement stunnedPlayerMovement;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Check if the collided object has a Health component
        Health playerHealth = collision.gameObject.GetComponent<Health>();
        if (playerHealth != null)
        {
            // Apply damage
            playerHealth.healthState(-damage);

            // Apply stun
            Movement playerMovement = collision.gameObject.GetComponent<Movement>();
            if (playerMovement != null && !isPlayerStunned)
            {
                stunnedPlayerMovement = playerMovement;
                StartCoroutine(StunPlayer(playerMovement));
            }
        }
    }

    private IEnumerator StunPlayer(Movement playerMovement)
    {
        Debug.Log("Player stunned.");
        isPlayerStunned = true;

        // Temporarily disable movement
        playerMovement.enabled = false;

        yield return new WaitForSeconds(stunDuration);

        if (isPlayerStunned) // Ensure the player is still stunned
        {
            playerMovement.enabled = true; // Re-enable movement
            Debug.Log("Player unstunned.");
            isPlayerStunned = false;
        }
    }

    private void OnDestroy()
    {
        // Ensure the player is unstunned if the enemy is destroyed
        if (isPlayerStunned && stunnedPlayerMovement != null)
        {
            stunnedPlayerMovement.enabled = true;
            Debug.Log("Player unstunned due to enemy destruction.");
            isPlayerStunned = false;
        }
    }
}