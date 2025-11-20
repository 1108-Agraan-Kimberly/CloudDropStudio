using UnityEngine;

public class hazard : MonoBehaviour
{
   public int damage = 1;

   private void OnCollisionEnter2D(Collision2D collision)
   {
    collision.gameObject.GetComponent<Health>().healthState(-damage);
   }
}
