using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public event Action<int> OnHealthChanged;

    public void healthState(int change)
    {
        currentHealth += change;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
