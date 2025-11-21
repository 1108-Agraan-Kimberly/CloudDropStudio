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
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isIdle", true);
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", false);
            return;
        }

        if (isCharging)
        {
            chargeTimer -= Time.fixedDeltaTime;
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
                if (Random.value < 0.01f) // ~1% chance per frame
                {
                    isCharging = true;
                    chargeTimer = chargeDuration;
                }
            }

            animator.SetBool("isMoving", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttacking", false);
        }

        float currentSpeed = isCharging ? chargeSpeed : speed;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * currentSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered range: " + collision.gameObject.name);
            player = collision.transform;
            isChasing = true;

            // Trigger the attack animation and deal damage
            animator.SetBool("isAttacking", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player exited range: " + collision.gameObject.name);
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isIdle", true);
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", false);
        }
    }
}