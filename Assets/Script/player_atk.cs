using UnityEngine;
using System.Collections;

public class player_atk : MonoBehaviour
{
    public Transform attackPoint;
    public GameObject sprite;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 2;
    public float fadeDuration = 0.5f;

    private SpriteRenderer opacity;

    void Start()
    {
        // try inspector-assigned sprite first
        if (sprite != null)
            opacity = sprite.GetComponent<SpriteRenderer>();

        // if not assigned, try to find a SpriteRenderer in attackPoint's children
        if (opacity == null && attackPoint != null)
            opacity = attackPoint.GetComponentInChildren<SpriteRenderer>();

    }

    public void Attack()
    {
        Collider2D[] Enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in Enemies)
        {
            // Try normal enemy first
            var eh = enemy.GetComponent<enemy_health>();
            if (eh != null)
            {
                eh.ChangeHealth(-attackDamage);
                continue;
            }

            // Try boss enemy
            var bh = enemy.GetComponent<BossHealth>();
            if (bh != null)
            {
                bh.ChangeHealth(-attackDamage);
                continue;
            }

            Debug.LogWarning($"player_atk: No health component found on {enemy.name}");
        }

        if (opacity != null)
        {
            StartCoroutine(ShowAttackTransparency());
        }

        Debug.Log("Player Attack!");
    }


    private IEnumerator ShowAttackTransparency()
    {
        Color original = opacity.color;
        Color halfAlpha = original;
        halfAlpha.a = 0.25f;
        opacity.color = halfAlpha;

        yield return new WaitForSeconds(fadeDuration);

        opacity.color = original;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
