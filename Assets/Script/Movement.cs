using UnityEngine;

public class Movement : MonoBehaviour
{
  public float speed;
  public float dashSpeed;
  public float dashDuration;

  private float dashTimeRemaining;
  private bool isDashing;

  public Transform aim;

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

    UpdateAimRotation();
  }

  private void UpdateAimRotation()
  {
    Vector3 mousePosition = Input.mousePosition;
    mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
    Vector3 aimDirection = mousePosition - aim.position;
    float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
    aim.rotation = Quaternion.Euler(0, 0, angle);
  }
}


