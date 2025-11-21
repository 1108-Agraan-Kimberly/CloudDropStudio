using UnityEngine;

public class Julian_ATK : MonoBehaviour
{
  public Animator animator;
  public player_atk playerAttackScript; // Reference to the player_atk script

    public void Attack()
    {
        animator.SetBool("isAttacking",true);

        // Trigger the shooting logic in player_atk
        if (playerAttackScript != null)
        {
            playerAttackScript.Shoot();
        }
    }

}
