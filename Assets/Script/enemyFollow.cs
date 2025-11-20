using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    private Animator animator;

    public float moveSpeed = 3f;
    public float attackDistance = 1.5f;

    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // If no player, enemy stays idle
        if (!isChasing || player == null)
        {
            SetIdle();
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);

        // If attacking, freeze movement and do NOT override animation state
        if (animator.GetBool("isAttacking"))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // ATTACK LOGIC
        if (dist < attackDistance)
        {
            StartAttack();
            return;
        }

        // CHASE LOGIC
        ChasePlayer();
    }

    // --------- CHASE BEHAVIOR ---------
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        animator.SetBool("isIdle", false);
        animator.SetBool("isMoving", true);
        animator.SetBool("isAttacking", false);
    }

    // --------- ATTACK BEHAVIOR ---------
    void StartAttack()
    {
        rb.linearVelocity = Vector2.zero;

        animator.SetBool("isMoving", false);
        animator.SetBool("isIdle", false);
        animator.SetBool("isAttacking", true);
    }

    // Called by animation event at end of attack
    public void FinishAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    // --------- IDLE BEHAVIOR ---------
    void SetIdle()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", false);
    }

    // --------- TRIGGER ENTER / EXIT ---------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered enemy trigger zone.");

            player = collision.transform;
            isChasing = true;

            animator.SetBool("isIdle", false);
            animator.SetBool("isMoving", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = false;
            player = null;

            SetIdle();
            rb.linearVelocity = Vector2.zero;
        }
    }
}
