using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public void healthState(int change)
    {
        currentHealth += change;
        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
