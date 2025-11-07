using UnityEngine;

public class enemyFollow : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Transform player;

    public float speed;
    private bool isChasing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing = true)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rigidbody.linearVelocity = direction * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(player == null)
            {
                player = collision.transform;
            }   
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rigidbody.linearVelocity = Vector2.zero;
            isChasing = false;
        }
    }
}
