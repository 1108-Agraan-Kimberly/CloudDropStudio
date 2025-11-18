using UnityEngine;

public class enemy_DMG : MonoBehaviour
{
    public int damage = 1;
    public Animator animator;
    public float weaponRange;
    public LayerMask playerLayer;
    public float attackCooldown = 1f;
    private float attackTimer;

    private void Update()
    {
        if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public void DealDamage()
    {
        if(attackTimer > 0)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, weaponRange, playerLayer);
            foreach(Collider2D hit in hits)
            {
                Health playerHealth = hit.GetComponent<Health>();
                if(hits.Length > 0)
                {
                    playerHealth.healthState(-damage);
                }
            }
            attackTimer = attackCooldown;
        }
  
    }

        public void FinishAttack()
    {
        animator.SetBool("isAttacking", false);
    }
}
