using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
  [SerializeField] private float speed = 5f;
  [SerializeField] private float dashSpeed = 15f;
  [SerializeField] private float dashDuration = 0.5f;
  [SerializeField] private float slowMultiplier = 0.5f; // Movement slowed by 50%
  private bool onSlowZone = false;


    private float dashTimeRemaining;
  private bool isDashing;

  private Vector2 moveInput;
  private Rigidbody2D rb;
  private Animator animator;

  public Vector2 LastFacingDirection { get; private set; } = Vector2.zero; 
  public player_atk playerAtk;

  [SerializeField] private float slipperyControl = 0.3f;  
  [SerializeField] private float slipperyDecay = 0.9f;     
  private bool onSlippery = false;                        
  private Vector2 slipperyVelocity;

  private bool isKnockedBack = false;
  private Vector2 knockbackVelocity;
  private float knockbackDurationRemaining;

  [SerializeField] private float knockbackDecay = 0.9f; 

    void Start() 
  {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
  }

  private void Update()
  {
    if(isDashing)
    {
      dashTimeRemaining -= Time.deltaTime;
      if(dashTimeRemaining <= 0f)
      {
        isDashing = false;
      }
    }
  }

  private void FixedUpdate()
  {
    if (isDashing)
    {
      rb.linearVelocity = moveInput.normalized * dashSpeed;
    }
    else if (isKnockedBack)
    {
      rb.linearVelocity = knockbackVelocity;
      knockbackVelocity *= knockbackDecay; // Gradually reduce knockback force

      knockbackDurationRemaining -= Time.fixedDeltaTime;
      if (knockbackDurationRemaining <= 0f)
      {
        isKnockedBack = false;
      }
    }
      //else if (!onSlippery)
        //{
        //  rb.linearVelocity = moveInput.normalized * speed;
        //}
        else if (!onSlippery)
        {
            float currentSpeed = speed;

            if (onSlowZone)
                currentSpeed *= slowMultiplier;

            rb.linearVelocity = moveInput.normalized * currentSpeed;
        }

        else
        {
        slipperyVelocity = rb.linearVelocity;

            // Apply input with reduced control
        Vector2 inputInfluence = moveInput.normalized * speed * slipperyControl;

            // Keep sliding but gradually slow down
        slipperyVelocity = slipperyVelocity * slipperyDecay + inputInfluence;

        rb.linearVelocity = slipperyVelocity;
        }
  }

  public void Move(InputAction.CallbackContext context)
  {
    moveInput = context.ReadValue<Vector2>();

    if(moveInput != Vector2.zero)
    {
      LastFacingDirection = moveInput.normalized; // Update facing direction only when moving
      animator.SetBool("isWalking", true);
    }
    else
    {
      animator.SetBool("isWalking", false);
    }

    animator.SetFloat("LastInputX", LastFacingDirection.x);
    animator.SetFloat("LastInputY", LastFacingDirection.y);
    animator.SetFloat("InputX", moveInput.x);
    animator.SetFloat("InputY", moveInput.y);
  }

  public void Dash(InputAction.CallbackContext context)
  {
    if(context.performed && !isDashing && moveInput != Vector2.zero)
    {
      isDashing = true;
      dashTimeRemaining = dashDuration;
    }
  }

  public void Attack(InputAction.CallbackContext context)
  {
    if(context.performed)
    {
      playerAtk.Attack();
    }
  }
    //  private void OnTriggerEnter2D(Collider2D collision)
    //  {
    //        if (collision.CompareTag("Slippery"))
    //        {
    //            onSlippery = true;
    //            Debug.Log("Entered slippery zone!");
    //        }
    //        if (collision.CompareTag("SlowZone"))
    //        {
    //            onSlowZone = true;
    //            Debug.Log("Entered slow zone!");
    //        }
    //    }

    //  private void OnTriggerExit2D(Collider2D collision)
    //  {
    //    if (collision.CompareTag("Slippery"))
    //    { 
    //        onSlippery = false;
    //        Debug.Log("Left slippery zone!");
    //    }

    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Slippery"))
        {
            onSlippery = true;
            Debug.Log("Entered slippery zone!");
        }

        if (collision.CompareTag("Slow"))
        {
            onSlowZone = true;
            Debug.Log("Entered slow zone!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Slippery"))
        {
            onSlippery = false;
            Debug.Log("Left slippery zone!");
        }

        if (collision.CompareTag("Slow"))
        {
            onSlowZone = false;
            Debug.Log("Left slow zone!");
        }
    }


    public void ApplyKnockback(Vector2 force, float duration)
  {
    isKnockedBack = true;
    knockbackVelocity = force;
    knockbackDurationRemaining = duration;
  }
}




