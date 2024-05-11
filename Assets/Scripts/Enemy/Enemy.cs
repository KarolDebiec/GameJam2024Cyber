using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{

    private GameController gameController;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] LayerMask palyerLayerMask;
    [SerializeField] GameObject player;
    public float attackTime;
    public Vector3 targetPos = new Vector3(0, 0, 0);
    public Vector3 translateSphereOnHight = new Vector3(0, 2, 0);
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
    private void Start()
    {
        speed = 6.0f;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        translateSphereOnHight = new Vector3(0, 3, 0);
        translateSphereOnGround = new Vector3(-1, -0.70f, 0);
        multipleGravity = 5.0f;
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
                    gameController.calculateSpeedup();
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

        Destroy(this.gameObject);
    }

    private void movement()
    {


        /// sprawdzenie y

        /// jesli rowny
        if (!isJumping)
        {

            Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(this.transform.position.x + this.translateAttackCircle.x, this.transform.position.y + this.translateAttackCircle.y), 0.3f, palyerLayerMask);
            if (hitCollider != null && isAttacked == false)
            {
                this.isAttacked = true;
            }

            if (Mathf.Abs(this.transform.position.y - player.transform.position.y) < 0.3f)
            {
                ///Ustawi� dobr� pozycje dla OverlapSphere
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x + this.translateSphereOnGround.x, this.transform.position.y + this.translateSphereOnGround.y), 0.3f, groundLayerMask);
                if (hitColliders.Length > 0)
                {

                    isTooHigh = false;
                    isNotEnoughtHeight = false;
                    this.targetPos = this.player.transform.position - this.transform.position;
                    this.targetPos.y = 0;
                    this.targetPos.z = 0;
                    this.targetPos.Normalize();

                    this.transform.position += (this.targetPos * Time.deltaTime) * speed;
                }
                else /// logika do niespadania kiedy przerwa mi�dzy platformiami
                {
                    //logika skoku
                    isJumping = true;
                    yVelocity = 25.0f;

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
                    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y + (-0.6f)), 0.3f, groundLayerMask);
                    if (hitColliders.Length > 0)
                    {
                        //skok w przepa��
                        if (hitColliders[0].CompareTag("Ground"))
                        {
                            this.targetPos.y = 0;
                            this.targetPos.Normalize();
                            this.transform.position += this.targetPos * Time.deltaTime * speed;
                        }
                        else
                        {
                            isJumping = true;
                            yVelocity = -1.5f;
                        }
                    }
                    else
                    {
                        isJumping = true;
                        yVelocity = 1.0f;
                    }


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
                    this.transform.position += this.targetPos * Time.deltaTime * speed;


                }
                else
                {
                    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x + this.translateSphereOnHight.x, this.transform.position.y + this.translateSphereOnHight.y), 0.3f, groundLayerMask);
                    if (hitColliders.Length > 0)
                    {
                        
                        this.isJumping = true;

                        yVelocity = 25.0f;

                    }
                    else { this.transform.position += this.targetPos * Time.deltaTime * speed; }

                }

                calculatexSphereTranslate();

            }
        }
        if (isJumping)
        {


            float posy = yVelocity * Time.deltaTime + yAcceleration * Time.deltaTime * Time.deltaTime / 2;
            float posx = this.targetPos.x * Time.deltaTime * speed;
            transform.position += new Vector3(posx, posy, 0);
            yVelocity = yVelocity + yAcceleration * multipleGravity * Time.deltaTime;

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y + (-0.80f)), 0.1f, groundLayerMask);
            if (hitColliders.Length > 0)
            {
                if (!hitColliders[0].CompareTag("Ground"))
                {
                    yVelocity = 0.0f; isJumping = false;

                    isNotEnoughtHeight = false;
                    isTooHigh = false;

                }


            };


        }
        // idzie x do gracza
        /// jesli mniejszy
        // sprawdzic czy platforma jest nad g�ow�
        //jesli nie dojsc do bezpiecznego
        //
        //jesli tak odejsc do punktu skoku

       
    }
        
    
        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(this.transform.position + translateSphereOnGround, 0.25f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(this.transform.position + translateSphereOnHight, 1.2f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(this.transform.position + new Vector3(0, -0.85f, 0), 0.1f);

            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(this.transform.position + new Vector3(translateAttackCircle.x, translateAttackCircle.y, 0), 0.3f);
        }

        void calculatexSphereTranslate()
        {
            if (targetPos.x > 0) { translateSphereOnGround = new Vector3(xSphereTranslate, -0.70f, 0f); translateAttackCircle = new Vector2(1, 0); }
            else { translateSphereOnGround = new Vector3(-xSphereTranslate, -0.70f, 0f); translateAttackCircle = new Vector2(-1, 0); }
        }


}
