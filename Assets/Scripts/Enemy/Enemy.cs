using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] GameObject player;
    public float attackTime;
    public Vector3 targetPos = new Vector3(0,0,0);
    public Vector3 translateSphereOnHight = new Vector3(0,2,0);
    public Vector3 translateSphereOnGround = new Vector3(-1, -1, 0);
    //float ySphereTranslate = 1;
    float xSphereTranslate = 1;
    public float speed = 0.5f;
    public float attackRange;
    public float attackSpeed;
    public float attackTimer;
    public float damage;
    public bool isFocusedOnPlayer;
    public bool playerIsInAttackRange;
    public float jumpHeight;
    public bool shouldJump;

    public bool isTooHigh = false;
    public bool isNotEnoughtHeight = false;

    public bool isAttacked;
    

    private void Start()
    {
        
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        this.movement();
        if (isAttacked)
        {
            attackTime -= Time.deltaTime;

            if (attackTime <= 0.0f)
            {
                attackTime = attackSpeed;
                isAttacked = false;
                // if (intersect)  //collider do ataku
                {
                    // this.attack();
                }
            }
        }
    }

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
       // this.player.takeDamage(damage);
    }

    public void takeDamage()
    {
        Destroy(this.gameObject);
    }

    private void movement()
    {
        
        
        /// sprawdzenie y

        /// jesli rowny
        if (Mathf.Abs(this.transform.position.y -player.transform.position.y) < 0.2f)
        {
            ///Ustawiæ dobr¹ pozycje dla OverlapSphere
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x + this.translateSphereOnGround.x, this.transform.position.y + this.translateSphereOnGround.y), 0.3f, groundLayerMask);
            if (hitColliders.Length > 0)
            {

                isTooHigh = false;
                isNotEnoughtHeight = false;
                this.targetPos = this.player.transform.position - this.transform.position;
                this.targetPos.y = 0;
                this.targetPos.Normalize();
                this.transform.position += this.targetPos * Time.deltaTime;
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
                this.targetPos = this.player.transform.position - this.transform.position;
                isTooHigh = true;
                this.targetPos.y = 0;
                this.targetPos.Normalize();
            }
            else
            {
                //opis ruchu
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y + (-1)), 0.3f, groundLayerMask);
                if(hitColliders.Length > 0)
                {
                    this.targetPos.y = 0;
                    this.targetPos.Normalize();
                    this.transform.position += this.targetPos * Time.deltaTime;
                }
                

            }
        }
        else if (this.transform.position.y < player.transform.position.y)
        {
            if (!this.isNotEnoughtHeight)
            {
                isNotEnoughtHeight = true;

                this.targetPos.y = 0;
                this.targetPos.Normalize();
            }
            else
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x + this.translateSphereOnHight.x, this.transform.position.y + this.translateSphereOnHight.y), 0.3f, groundLayerMask);
                if (hitColliders.Length > 0)
                {
                    this.transform.position += this.targetPos * Time.deltaTime * speed;
                    
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
        calculatexSphereTranslate();

    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(this.transform.position + translateSphereOnGround, 0.6f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(this.transform.position + translateSphereOnHight, 0.6f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(this.transform.position + new Vector3(0,-1,0), 0.6f);
    }

    void calculatexSphereTranslate()
    {
        if(targetPos.x>0) { translateSphereOnGround = new Vector3(xSphereTranslate,-1f,0f); }
        else  { translateSphereOnGround = new Vector3(-xSphereTranslate,-1f,0f); }
    }

}
