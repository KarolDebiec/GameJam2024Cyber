using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{

    private GameController gameController;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] LayerMask palyerLayerMask;
   Transform player;
    [SerializeField] Animator animator;
    public float attackTime;
    public Vector3 targetPos = new Vector3(0, 0, 0);
    public Vector3 translateSphereOnHight = new Vector3(0, 3, 0);
    public Vector3 translateSphereOnGround = new Vector3(-1, -0.7f, 0);
    public Vector2 translateAttackCircle = new Vector2(0, 0);
    //float ySphereTranslate = 1;
    float xSphereTranslate = 1;
    public float speed;
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
    [SerializeField] TagAttribute tag;


    public bool isJumping=false;
    public float yVelocity = 0.0f;
    public float yAcceleration = -10f;
    public float multipleGravity = 2.0f;

    public bool isRotated = false;
    private void Start()
    {
        speed = 6.0f;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        
        translateSphereOnHight = new Vector3(0, 3, 0);
        translateSphereOnGround = new Vector3(-1, -0.70f, 0);
        multipleGravity = 5.0f;
        attackSpeed = 0.2f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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
                Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(this.transform.position.x + this.translateAttackCircle.x, this.transform.position.y + this.translateAttackCircle.y), 0.3f, palyerLayerMask);
                if (hitCollider != null)
                {
                    animator.SetTrigger("enemyAttack");
                    gameController.calculateSpeedup();
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>().PlayEnemyAttackSoundClip();
                }
                attackTime = attackSpeed;
                isAttacked = false;
                // if (intersect)  //collider do ataku
                {
                    // this.attack();
                }
            }
        }
    }


    public void takeDamage()
    {
        gameController.calculateSpeeddown();
        GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>().PlayEnemyDeathSoundClip();
        Destroy(this.gameObject);
    }

    private void movement()
    {
        this.targetPos = this.player.position - this.transform.position;
        calculatexSphereTranslate();
        this.targetPos.y = 0;
        this.targetPos.z = 0;
        this.targetPos.Normalize();
        /// sprawdzenie y

        /// jesli rowny
        if (!isJumping)
        {

            Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(this.transform.position.x + this.translateAttackCircle.x, this.transform.position.y + this.translateAttackCircle.y), 0.3f, palyerLayerMask);
            if (hitCollider != null && isAttacked == false)
            {
                this.isAttacked = true;

            }

            if (Mathf.Abs(this.transform.position.y - player.position.y) < 0.3f)
            {
                ///Ustawiæ dobr¹ pozycje dla OverlapSphere
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x + this.translateSphereOnGround.x, this.transform.position.y + this.translateSphereOnGround.y), 0.3f, groundLayerMask);
                if (hitColliders.Length > 0)
                {

                    isTooHigh = false;
                    isNotEnoughtHeight = false;
                    this.targetPos = this.player.position - this.transform.position;
                    this.targetPos.y = 0;
                    this.targetPos.z = 0;
                    this.targetPos.Normalize();

                    this.transform.position += (this.targetPos * Time.deltaTime) * speed;
                }
                else /// logika do niespadania kiedy przerwa miêdzy platformiami
                {
                    //logika skoku
                    isJumping = true;
                    yVelocity = 25.0f;

                }

            }
            //biegnie dopoki nie zaatakuje lub nie zginie

            /// jesli wiekszy
            else if (this.transform.position.y > player.position.y)
            {
                

                  
                 isJumping = true;
                yVelocity = -1.5f;
                isTooHigh = false;
                   
                        
                   
            }
            else if (this.transform.position.y < player.transform.position.y)
            {

                if (!this.isNotEnoughtHeight)
                {

                    isNotEnoughtHeight = true;
                    this.targetPos = this.player.transform.position - this.transform.position;
                    this.targetPos.y = 0;
                    this.targetPos.Normalize();
                    this.transform.position += this.targetPos * Time.deltaTime * speed;


                }
                else
                {
                    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x + this.translateSphereOnHight.x, this.transform.position.y + this.translateSphereOnHight.y), 2f, groundLayerMask);
                    if (hitColliders.Length > 0)
                    {
                        this.targetPos = this.player.transform.position - this.transform.position;
                        this.targetPos.Normalize();
                        this.isJumping = true;

                        yVelocity = 25.0f;

                    }
                    else {
                        Collider2D[] col = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x + this.translateSphereOnGround.x, this.transform.position.y + this.translateSphereOnGround.y), 0.3f, groundLayerMask);
                        if (!(col.Length > 0) && this.transform.position.y> 2.5f)
                        {
                            isJumping = true;
                            yVelocity = 35.0f;
                        }
                            this.transform.position += this.targetPos * Time.deltaTime * speed; 
                    }

                }

                calculatexSphereTranslate();

            }
        }
       else if (isJumping)
        {


            float posy = yVelocity * Time.deltaTime + yAcceleration * Time.deltaTime * Time.deltaTime / 2;
            float posx = this.targetPos.x * Time.deltaTime * speed;
            transform.position += new Vector3(posx, posy, 0);
            yVelocity = yVelocity + yAcceleration * multipleGravity * Time.deltaTime;
            isNotEnoughtHeight = false;
            isTooHigh = false;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y + (-0.80f)), 0.1f, groundLayerMask);
            if (hitColliders.Length > 0)
            {
               
                    yVelocity = 0.0f; isJumping = false;



            };


        }
        // idzie x do gracza
        /// jesli mniejszy
        // sprawdzic czy platforma jest nad g³ow¹
        //jesli nie dojsc do bezpiecznego
        //
        //jesli tak odejsc do punktu skoku

       
    }
        
    
        void OnDrawGizmos()
        {
           /* // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(this.transform.position + translateSphereOnGround, 0.25f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(this.transform.position + translateSphereOnHight, 2.0f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(this.transform.position + new Vector3(0, -0.85f, 0), 0.1f);

            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(this.transform.position + new Vector3(translateAttackCircle.x, translateAttackCircle.y, 0), 0.3f);*/
        }

        void calculatexSphereTranslate()
        {
            if (targetPos.x > 0) {

            translateSphereOnGround = new Vector3(xSphereTranslate, -0.70f, 0f); translateAttackCircle = new Vector2(0.7f, 0);

            if (isRotated)
            {
                this.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
                isRotated = false;
            }
        }
            else {
            translateSphereOnGround = new Vector3(-xSphereTranslate, -0.70f, 0f); translateAttackCircle = new Vector2(-0.7f, 0);
            
          

            if (!isRotated)
            {
                this.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
                isRotated = true;
            }
        }
        }


}
