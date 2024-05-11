using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationVariablesChangeBody : MonoBehaviour
{

    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator legsAnimator;
    [SerializeField] private PlayerCombat playerCombat;

    

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(playerCombat.attackPoint.position, playerCombat.attackRange, playerCombat.enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            //Logika zabierania zycia
            Debug.Log("hit");
            enemy.GetComponent<Enemy>().takeDamage();
        }
    }

}
