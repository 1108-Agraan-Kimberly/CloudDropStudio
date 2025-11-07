using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    private bool isDead = false;

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Color hitFlashColor = Color.red;
    public float flashDuration = 0.1f;

    void Start()
    {
        currentHealth = maxHealth;
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeHealth(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (amount < 0)
        {
            StartCoroutine(FlashOnHit());
            Debug.Log($"Boss took {-amount} damage! Remaining HP: {currentHealth}");
        }

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator FlashOnHit()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = hitFlashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Boss defeated!");

        if (animator != null)
            animator.SetTrigger("Die");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 2f);
    }
}
