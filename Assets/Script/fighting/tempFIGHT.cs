using UnityEngine;

public class tempFIGHT : MonoBehaviour
{   
    public int damage = 1;
    public Transform attackPoint;
    public float attackRange;
    public LayerMask playerLayer;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().healthState(-damage);
        }
        
    }

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        if(hits.Length > 0)
        {
            hits[0].GetComponent<Health>().healthState(-damage);
        }
        Debug.Log("Attacking");
    }
 
}
