using UnityEngine;

public class enemy_DMG : MonoBehaviour
{
    public int damage = 1;
    public Animator animator;
    public float weaponRange = 1f;
    public LayerMask playerLayer;
    public float attackCooldown = 1f;
    private float attackTimer;

    [Header("Optional")]
    public Transform attackOrigin; // use this transform as origin for overlap; falls back to enemy transform

    public bool debugLogs = true;

    private void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    // Called by animation event or code when the enemy should apply its hit
    public void DealDamage()
    {
        if (attackTimer > 0)
        {
            if (debugLogs) Debug.Log($"enemy_DMG.DealDamage: on cooldown ({attackTimer:F2}s left).");
            return;
        }

        if (debugLogs) Debug.Log("enemy_DMG.DealDamage: called");

        Vector2 origin = (attackOrigin != null) ? (Vector2)attackOrigin.position : (Vector2)transform.position;

        // First try with the player layer mask
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, weaponRange, playerLayer);
        if (debugLogs) Debug.Log($"enemy_DMG.DealDamage: layer query found {hits.Length} colliders.");

        // Fallback to unfiltered query if nothing found (helps if layer is misconfigured)
        if (hits == null || hits.Length == 0)
        {
            hits = Physics2D.OverlapCircleAll(origin, weaponRange);
            if (debugLogs) Debug.Log($"enemy_DMG.DealDamage: fallback query found {hits.Length} colliders.");
        }

        bool hitAny = false;

        foreach (Collider2D hit in hits)
        {
            if (hit == null) continue;

            // Prefer explicit player tag to avoid hitting other objects
            if (!hit.CompareTag("Player"))
            {
                if (debugLogs) Debug.Log($"enemy_DMG.DealDamage: skipping '{hit.name}' (tag='{hit.tag}')");
                continue;
            }

            // Find Health reliably (collider, parent, children)
            Health playerHealth = hit.GetComponent<Health>();
            if (playerHealth == null) playerHealth = hit.GetComponentInParent<Health>();
            if (playerHealth == null) playerHealth = hit.GetComponentInChildren<Health>();

            if (playerHealth != null)
            {
                if (debugLogs) Debug.Log($"enemy_DMG.DealDamage: applying {damage} to '{playerHealth.gameObject.name}' (collider: '{hit.name}').");
                playerHealth.healthState(-damage);
                hitAny = true;
            }
            else
            {
                if (debugLogs) Debug.Log($"enemy_DMG.DealDamage: '{hit.name}' has no Health component.");
            }
        }

        if (!hitAny && debugLogs) Debug.Log("enemy_DMG.DealDamage: no player hit. Check player tag/layer/weaponRange/attackOrigin and that DealDamage() is called.");

        attackTimer = attackCooldown;
    }

    public void FinishAttack()
    {
        if (animator != null)
            animator.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 origin = (attackOrigin != null) ? attackOrigin.position : transform.position;
        Gizmos.DrawWireSphere(origin, weaponRange);
    }
}
