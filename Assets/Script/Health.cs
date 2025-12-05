using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public DeathMenu deathMenu;

    public event Action<int> OnHealthChanged;

    public void healthState(int change)
    {
        Debug.Log($"Health before change: {currentHealth}");
        currentHealth += change;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Health after change: {currentHealth}");

        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Health reached 0. Reloading scene.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //if (currentHealth <= 0)
        //{
        //    Debug.Log("Health reached 0. Triggering DIE()");
        //    Die();  
        //}
    }

    public void Die()
    {
        deathMenu.ShowDeathMenu();
    }

}
