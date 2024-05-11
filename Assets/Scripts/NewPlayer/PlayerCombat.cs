using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator legsAnimator;
    //[SerializeField] private float timeBetweenAttacks = 0f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    public CharacterController2D characterController;
    private bool isAttacking;

    

    private void Start()
    {
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isAttacking)
                Attack();
        }
        
    }

    private void Attack()
    {
        isAttacking = true;
        bodyAnimator.SetTrigger("attack");
        GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>().PlayPlayerAttackSoundClip();
        //yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
