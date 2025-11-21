using UnityEngine;

public class PowerUpCollector : MonoBehaviour
{
    // Reference to the specific ScriptableObject asset instance in the project
    public PowerUpSO powerUpEffect;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Apply the effect defined in the ScriptableObject to the player GameObject
            powerUpEffect.ApplyEffect(other.gameObject);
            Destroy(gameObject); // Destroy the physical power-up object in the scene
        }
    }
}
