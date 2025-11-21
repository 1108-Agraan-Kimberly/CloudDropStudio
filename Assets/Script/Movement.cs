using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
  [SerializeField] private float speed = 5f;
  [SerializeField] private float dashSpeed = 15f;
  [SerializeField] private float dashDuration = 0.5f;

  private float dashTimeRemaining;
  private bool isDashing;

  private Vector2 moveInput;
  private Rigidbody2D rb;
  private Animator animator;

  public Vector2 LastFacingDirection { get; private set; } = Vector2.zero; // Default to zero
  public player_atk playerAtk;

  [SerializeField] private float slipperyControl = 0.3f;  // How much control you have on ice
  [SerializeField] private float slipperyDecay = 0.9f;     // How fast sliding slows down
  private bool onSlippery = false;                        // Are we standing on slippery floor?
  private Vector2 slipperyVelocity;

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
    if(isDashing)
    {
      rb.linearVelocity = moveInput.normalized * dashSpeed;
    }
    if(!onSlippery)
    {
      rb.linearVelocity = moveInput.normalized * speed;
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
  private void OnTriggerEnter2D(Collider2D collision)
  {
        if (collision.CompareTag("Slippery"))
        {
            onSlippery = true;
            Debug.Log("Entered slippery zone!");
        }
   }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.CompareTag("Slippery"))
    { 
        onSlippery = false;
        Debug.Log("Left slippery zone!");
    }
}
}




