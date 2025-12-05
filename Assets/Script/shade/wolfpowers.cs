using UnityEngine;

public class wolfpowers : MonoBehaviour
{
    public float speed = 6f;

    // Damage amount when this projectile hurts the player
    public int damage = 1;

    // Healing amount when this projectile heals the player
    public int healAmount = 1;

    // When true, projectile will heal (adds health); otherwise it damages (subtracts)
    public bool healsPlayer = false;

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
            if (healsPlayer)
                playerHealth.healthState(healAmount);   // positive -> heal
            else
                playerHealth.healthState(-damage);     // negative -> damage

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
