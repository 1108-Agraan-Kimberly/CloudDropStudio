using UnityEngine;
using System.Collections;

public class player_atk : MonoBehaviour
{
    public Animator animator;

    public void Attack()
    {
        animator.SetBool("isAttacking",true);
    }

    public void FinishAttack()
    {
        animator.SetBool("isAttacking", false);
    }
}



