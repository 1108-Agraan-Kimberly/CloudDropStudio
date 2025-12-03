using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health hp = collision.GetComponent<Health>();
            if (hp != null)
            {
                hp.healthState(-damage);
            }
            Destroy(gameObject);
        }

        // Destroy bullet on hitting walls or obstacles
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
