using UnityEngine;

public class player_atk : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 2;

    public void Attack()
    {

    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Health>()?.healthState(-attackDamage);
        }
    Debug.Log("Player Attack!");
    }
}
