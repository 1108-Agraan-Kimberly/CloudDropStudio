using UnityEngine;

public class Movement : MonoBehaviour
{
  public float speed;
  public float dashSpeed;
  public float dashDuration;

  private float dashTimeRemaining;
  private bool isDashing;

  private void Update()
  {
    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");

    Vector3 direction = new Vector3(horizontal, vertical);
    if (direction.magnitude > 0)
    {
      direction.Normalize();
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
  }


}


