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
    animator.SetBool("isWalking", true);

    if(context.canceled)
    {
      animator.SetBool("isWalking", false);
      animator.SetFloat("LastInputX", moveInput.x);
      animator.SetFloat("LastInputY", moveInput.y);
    }

    moveInput = context.ReadValue<Vector2>();
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




