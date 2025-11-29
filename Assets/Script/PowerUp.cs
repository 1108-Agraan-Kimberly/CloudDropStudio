using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int healAmount = 2;
    //public PotionCounter potionCounter;

    // private void Awake()
    // {
    //     // Find the PotionCounter in the scene
    //     potionCounter = FindObjectOfType<PotionCounter>();

    //     if (potionCounter == null)
    //     {
    //         Debug.LogWarning("PotionCounter not found in scene. Make sure one exists on your ESC menu Canvas.");
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();

        if (health != null)
        {
            health.healthState(healAmount);
            // if (PotionCounter.Instance != null)
            // {
            //     PotionCounter.Instance.AddPotion();
            // }
            Destroy(gameObject);
        }
    }
}
