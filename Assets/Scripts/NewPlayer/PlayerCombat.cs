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
    private bool shouldAttack;

    

    private void Start()
    {
        isAttacking = false;
        shouldAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isAttacking)
                Attack();
        }
    }

    private void Attack()
    {
        if(!GetComponent<CharacterController2D>().isPlayerDead)
        {
            isAttacking = true;
            bodyAnimator.SetTrigger("attack");
            GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>().PlayPlayerAttackSoundClip();

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach (Collider2D enemy in hitEnemies)
            {
                //Logika zabierania zycia
                Debug.Log("hit");
                enemy.GetComponent<Enemy>().takeDamage();
            }
            isAttacking = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
