using UnityEngine;

public class enemy_movement : MonoBehaviour
{
    public float speed;
    
    private Rigidbody2D rb;
    private Transform player;
    private int facingDirection = -1;
    private SpriteRenderer sr;
    private Animator a;
    private EnemyState enemyState, newState;
    public float attackRange = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if(enemyState == EnemyState.Walking)
        {

            Chase();
            
        }
        else if(enemyState == EnemyState.Attacking)
        {
           rb.linearVelocity = Vector2.zero;
        }

    }

    void Chase()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            ChangeState(EnemyState.Attacking);
            //rb.linearVelocity = Vector2.zero; // Stop movement when in attack range
        }
        else if(player.position.x > transform.position.x && facingDirection == -1
            || player.position.x < transform.position.x && facingDirection == 1)
            {
                Debug.Log("Flipping enemy!");
                Flip();
            }
            
                Vector2 direction = (player.position - transform.position).normalized;
                rb.linearVelocity = direction * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(player == null)
            {
                player = collision.transform; 
            }
            ChangeState(EnemyState.Walking);
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            rb.linearVelocity = Vector2.zero; // Stop movement when player exits range 
            ChangeState(EnemyState.Walking);
        }

    }

    void Flip()
    {
        facingDirection *= -1; // keep your facing logic if you need it
        if (sr != null)
        {
            sr.flipX = !sr.flipX;
            Debug.Log("Enemy flipped via flipX: now facing " + (facingDirection == 1 ? "Right" : "Left"));
        }
        else
        {
            // fallback if no SpriteRenderer found
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            Debug.Log("Enemy flipped via localScale fallback");
        }
    }
    void ChangeState(EnemyState newState)
    {
        // Exit current state
        if(enemyState == EnemyState.Idle)
            a.SetBool("isIdle", false);
        else if(enemyState == EnemyState.Walking)
            a.SetBool("isWalking", false);
        else if(enemyState == EnemyState.Attacking)
            a.SetBool("isAttacking", false);            
        //update to new state
        enemyState = newState;
        //update animator
        if(enemyState == EnemyState.Idle)
            a.SetBool("isIdle", true);
        else if(enemyState == EnemyState.Walking)
            a.SetBool("isWalking", true);
        else if(enemyState == EnemyState.Attacking)
            a.SetBool("isAttacking", true); 
    }
}

public enum EnemyState
{
    Idle,
    Walking,
    Attacking
}