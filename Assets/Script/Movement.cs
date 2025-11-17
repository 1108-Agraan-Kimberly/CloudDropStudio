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
    else
    {
      rb.linearVelocity = moveInput.normalized * speed;
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
}




