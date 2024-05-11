using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator legsAnimator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    public CharacterController2D characterController;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }
        
    }

    void Attack()
    {
        bodyAnimator.SetBool("isAttacking", true);
        if (characterController.horizontalMove == 0f)
        {
            legsAnimator.SetBool("isAttacking", true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void StopAttacking()
    {
        bodyAnimator.SetBool("isAttacking", false);
    }
}
