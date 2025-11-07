using UnityEngine;

public class Movement : MonoBehaviour
{
  public float speed;
  public float dashSpeed;
  public float dashDuration;

  private float dashTimeRemaining;
  private bool isDashing;
  private Vector3 lastDirection = Vector3.right;

  private SpriteRenderer SpriteRenderer;
  public player_atk playerAtk; 


  private void Awake()
  {
    SpriteRenderer = GetComponent<SpriteRenderer>();
  }

  private void Update()
  {
    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");

    Vector3 direction = new Vector3(horizontal, vertical);
    if (direction.magnitude > 0)
    {
      direction.Normalize();
      lastDirection = direction;

    if(horizontal != 0)
      {
        Flip(horizontal);
      }

    }
  
    if (Input.GetKeyDown(KeyCode.Space) && !isDashing) // Dash input
    {
      isDashing = true;
      dashTimeRemaining = dashDuration;
    }

    if (isDashing)
    {
      transform.position += direction * dashSpeed * Time.deltaTime;
      dashTimeRemaining -= Time.deltaTime;

      if (dashTimeRemaining <= 0)
      {
        isDashing = false;
      }
    }
    else
    {
      transform.position += direction * speed * Time.deltaTime;
    }

    if(Input.GetMouseButtonDown(0))
    {
      playerAtk?.Attack(); //call the Attack method from player_atk script
    }
  }

  private void Flip(float horizontal)
  {
    if(SpriteRenderer != null)
    {
      SpriteRenderer.flipX = horizontal < 0 ;
    }
    else
    {
      Vector3 scale = transform.localScale;
      scale.x = (horizontal < 0) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
      transform.localScale = scale;
    }
        
    }
  }





