using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int healAmount = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object we collided with is the player
        Health health = collision.GetComponent<Health>();

        if (health != null)
        {
            health.healthState(healAmount);
            Destroy(gameObject);
        }
    }
}
