using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;

    public float speed = 3f;
    private bool isChasing = false;

    public float chargeSpeed = 8f;
    public float chargeDuration = 1f;
    public float chargeCooldown = 4f;

    private bool isCharging = false;
    private float chargeTimer = 0f;
    private float cooldownTimer = 0f;

    private Animator animator;
    private enemy_DMG enemyDamageScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Reference the Animator
        enemyDamageScript = GetComponent<enemy_DMG>(); // Reference the enemy_DMG script
    }

    void FixedUpdate()
    {
        if (!isChasing || player == null)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isIdle", true);
            animator.SetBool("isMoving", false);
            animator.SetBool("isCharging", false);
            animator.SetBool("isAttacking", false);
            return;
        }

        // Handle cooldowns
        if (isCharging)
        {
            chargeTimer -= Time.fixedDeltaTime;
            animator.SetBool("isCharging", true);
            animator.SetBool("isMoving", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttacking", false);

            if (chargeTimer <= 0f)
            {
                isCharging = false;
                cooldownTimer = chargeCooldown;
            }
        }
        else
        {
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.fixedDeltaTime;
            }
            else
            {
                // Randomly decide to charge
                if (Random.value < 0.01f) // ~1% chance per frame
                {
                    isCharging = true;
                    chargeTimer = chargeDuration;
                }
            }

            animator.SetBool("isCharging", false);
            animator.SetBool("isMoving", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttacking", false);
        }

        float currentSpeed = isCharging ? chargeSpeed : speed;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * currentSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            isChasing = true;
            animator.SetBool("isAttacking", true);

            // Call the enemy_DMG script to deal damage
            enemyDamageScript.OnCollisionEnter2D(new Collision2D());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = false;
            rb.velocity = Vector2.zero;
            animator.SetBool("isIdle", true);
            animator.SetBool("isMoving", false);
            animator.SetBool("isCharging", false);
            animator.SetBool("isAttacking", false);
        }
    }
}