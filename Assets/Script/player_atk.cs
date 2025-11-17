using UnityEngine;
using System.Collections;

public class player_atk : MonoBehaviour
{
    public Animator animator;
    public float cooldown = 0.5f;
    private float timer;
    public Transform range;
    public float attackRange = 2f;
    public LayerMask enemyLayer;
    public int damage = 1;

    private Movement movementScript;

    private void Start()
    {
        movementScript = GetComponent<Movement>(); // Reference the Movement script
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (timer <= 0)
        {
            animator.SetBool("isAttacking", true);

            // Update the range position based on the player's facing direction
            Vector2 attackDirection = movementScript.LastFacingDirection;
            if (attackDirection != Vector2.zero) // Ensure a valid direction
            {
                range.localPosition = attackDirection * attackRange;
            }

            // Detect enemies in the attack range
            Collider2D[] enemies = Physics2D.OverlapCircleAll(range.position, attackRange, enemyLayer);
            foreach (Collider2D enemy in enemies)
            {
                enemy.GetComponent<enemy_health>().ChangeHealth(-damage);
            }

            timer = cooldown;
        }
    }

    public void FinishAttack()
    {
        animator.SetBool("isAttacking", false);
    }
}



