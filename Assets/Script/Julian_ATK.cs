using UnityEngine;

public class Julian_ATK : MonoBehaviour
{
  public Animator animator;

    public void Attack()
    {
        animator.SetBool("isAttacking",true);
    }

}
