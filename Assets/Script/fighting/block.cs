using UnityEngine;

public class block : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public spellbook SB;
    private SpriteRenderer sr;
    private void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if(SB.collected) //if spellbook is collected
        {
            if(amount < 0)
            {
                StartCoroutine(Red());
            }
            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }   
        }
        
    }
        
    private System.Collections.IEnumerator Red()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
}
