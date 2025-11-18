using UnityEngine;

public class enemy_DMG : MonoBehaviour
{
    public int damage = 1;

    public void OnCollisionEnter2D(Collision2D collision)
    {
       collision.gameObject.GetComponent<Health>().healthState(-damage);
    }
}
