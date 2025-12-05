using UnityEngine;

public class wolfpowers : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 1;
    private Vector2 direction;

    public float lifetime = 4f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Health playerHealth = collision.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.healthState(-damage);
            Destroy(gameObject);
            return;
        }


        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
