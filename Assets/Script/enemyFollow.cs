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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!isChasing || player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Handle cooldowns
        if (isCharging)
        {
            chargeTimer -= Time.fixedDeltaTime;
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
        }

        float currentSpeed = isCharging ? chargeSpeed : speed;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * currentSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }
}