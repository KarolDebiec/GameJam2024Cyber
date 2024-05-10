using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public Player player;
    public Vector3 targetPos;
    public float range;
    public float speed;
    public float attackRange;
    public float attackSpeed;
    public float attackTimer;
    public float damage;
    public bool isFocusedOnPlayer;
    public bool playerIsInAttackRange;

    public float jumpHeight;


    // Update is called once per frame
    void Update()
    {
        
    }

    public void patroling()
    {

    }

    public void chasePlayer()
    {

    }

    public void attack()
    {
        this.player.takeDamage(damage);
    }

    public void takeDamage()
    {
        Destroy(this.gameObject);
    }

}
