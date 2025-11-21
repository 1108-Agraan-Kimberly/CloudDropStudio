using UnityEngine;

public class enemy_movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform player;

    void Start()
    {
        rb = GetComponent<RigidBody2d>();
    }

    void Update()
    {
        Vector2 direction = (player.position - transform.position).normalized;
    }
}