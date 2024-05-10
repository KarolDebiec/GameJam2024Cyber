using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    /*
    [SerializeField] public Player player;
    [SerializeField] BoxCollider2D colliderForJumping;
    [SerializeField] BoxCollider2D colliderForFalling;
    public float attackTime;
    public Vector3 targetPos;
    public float speed;
    public float attackRange;
    public float attackSpeed;
    public float attackTimer;
    public float damage;
    public bool isFocusedOnPlayer;
    public bool playerIsInAttackRange;
    public float jumpHeight;
    public bool shouldJump;

    public bool isTooHigh;
    public bool isNotEnoughtHeight;

    public bool isAttacked;
    [SerializeField] public LayerMask groundLayer;

    // Update is called once per frame
    void Update()
    {
        if (isAttacked)
        {
            attackTime -= Time.deltaTime;

            if (attackTime <= 0.0f)
            {
                timerEnded();
                attackTime = attackSpeed;
                isAttacked = false;
                if (intersect)  //collider do ataku
                {
                    this.attack();
                }
            }
        }

        this.movement();
        
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

    private void movement()
    {


        /// sprawdzenie y

        /// jesli rowny
        if (this.transform.position.y == player.transform.position.y)
        {
            ///Ustawiæ dobr¹ pozycje dla OverlapSphere
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 0.5f, 6);
            if (hitColliders.Length > 0)
            {
                isTooHigh = false;
                isNotEnoughtHeight = false;
                this.targetPos = this.player.transform.position - this.transform.position;
            }
            else /// logika do niespadania kiedy przerwa miêdzy platformiami
            {

            }

        }
        //biegnie dopoki nie zaatakuje lub nie zginie

        /// jesli wiekszy
        else if (this.transform.position.y > player.transform.position.y)
        {
            if (!this.isTooHigh)
            {
                isTooHigh = true;
                this.targetPos = this.player.transform.position - this.transform.position;
                this.targetPos.y = 0;
                this.targetPos.Normalize();
            }
            else
            {
                //opis ruchu
                this.transform.position += this.targetPos * Time.deltaTime;

            }
        }
        else if (this.transform.position.y < player.transform.position.y)
        {
            if (!this.isNotEnoughtHeight)
            {
                isNotEnoughtHeight = true;
                this.targetPos = this.player.transform.position - this.transform.position;
                this.targetPos.y = 0;
                this.targetPos.Normalize();
            }
            else
            {
                Collider[] hitColliders = Physics.OverlapSphere(this.transform.position + new Vector3(0.0f,1.0f,0.0f), 0.5f, 6);
                if (hitColliders.Length > 0)
                {
                    this.transform.position += this.targetPos * Time.deltaTime;
                }
                else ///logika do skoku na platforme
                {

                }
            }
        }
            // idzie x do gracza
            /// jesli mniejszy
            // sprawdzic czy platforma jest nad g³ow¹
            //jesli nie dojsc do bezpiecznego
            //
            //jesli tak odejsc do punktu skoku
        }

    */

}
