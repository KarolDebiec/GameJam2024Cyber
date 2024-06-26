using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : MonoBehaviour
{

    private GameController gameController;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] LayerMask palyerLayerMask;
    [SerializeField] Transform player;
    public Animator animator;
    public float attackTime;
    public Vector3 targetPos = new Vector3(0, 0, 0);
    public Vector3 translateSphereOnHight = new Vector3(0, 2, 0);
    public Vector3 translateSphereOnGround = new Vector3(-1, -1, 0);
    public Vector2 translateAttackCircle = new Vector2(0, 0);
    [SerializeField] GameObject firstBody;
    [SerializeField] GameObject secBody;
    [SerializeField] GameObject lastBody;
    [SerializeField] GameObject blood;
    //float ySphereTranslate = 1;
    float xSphereTranslate = 1;
    private float MaxsAttackpeed=1.0f;
    private float AttackpeedMultiply=1.0f;
    public float speed;
    public float Maxspeed = 6.0f;
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
    [SerializeField] public GameObject myPrefab;
    public bool isJumping;
    public float yVelocity = 0.0f;
    public float yAcceleration = -9.81f;

    public bool canMove;
    public float distanceToPlayer;

    public GameObject onDeathObject;
    private void Start()
    {
        
        speed = 6.0f;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.distanceToPlayer = 15.0f;
        attackSpeed = 1.5f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    // Update is called once per frame
    public void Throw()
    {
        animator.SetTrigger("enemyAttack");
        
        GameObject newObject=Instantiate(myPrefab, this.transform.position, Quaternion.identity);
       
        GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>().PlayEnemyAttackSoundClip();
    }
    private void FixedUpdate()
    {
        animator.speed = speed/Maxspeed;
        Vector3 helper = player.transform.position - this.transform.position;
        if (helper.magnitude >= distanceToPlayer)
        {
            this.movement();
        }
        else
        {
            this.reverseMovement();
        }
        if (transform.position.y < 2.0f){Vector3 newPosition = new Vector3(transform.position.x, 2.0f, transform.position.z);
            transform.position = newPosition;
        }
        attackTime -= Time.deltaTime;

        if (helper.magnitude <= distanceToPlayer + 5.0f)
        {


            if (attackTime <= 0.0f)
            {

                attackTime = 3.0f;
                this.Throw();
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
        Instantiate(onDeathObject, gameObject.transform.position, Quaternion.identity);
        int value = Random.Range(1, 4);
        if(1 == value) Instantiate(firstBody, this.transform.position, Quaternion.identity);
        else if(2 == value) Instantiate(secBody, this.transform.position, Quaternion.identity);
        else if(3 == value)Instantiate(lastBody, this.transform.position, Quaternion.identity);
        Instantiate(blood, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void movement()
    {


        /// sprawdzenie y

        /// jesli rowny
        if (!isJumping)
        {


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
                    yVelocity = 10.0f;

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
                    if (hitColliders.Length > 0)
                    {
                        //skok w przepa��
                        this.targetPos.y = 0;
                        this.targetPos.Normalize();
                        this.transform.position += this.targetPos * Time.deltaTime * speed;
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

                    this.targetPos.y = 0;
                    this.targetPos.Normalize();
                }
                else
                {
                    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x + this.translateSphereOnHight.x, this.transform.position.y + this.translateSphereOnHight.y), 0.3f, groundLayerMask);
                    if (hitColliders.Length > 0)
                    {
                        this.isJumping = true;

                        yVelocity = 13.0f;

                    }
                    else ///logika do skoku na platforme
                    {
                        this.transform.position += this.targetPos * Time.deltaTime * speed;
                    }
                }
            }
            calculatexSphereTranslate();
        }
        else if (isJumping)
        {

            float posy = yVelocity * Time.deltaTime + yAcceleration * Time.deltaTime * Time.deltaTime / 2;
            float posx = this.targetPos.x * Time.deltaTime * speed;
            transform.position += new Vector3(posx, posy, 0);
            yVelocity = yVelocity + yAcceleration * Time.deltaTime;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x + this.translateSphereOnGround.x, this.transform.position.y + this.translateSphereOnGround.y), 0.3f, groundLayerMask);
            if (hitColliders.Length > 0) { yVelocity = 0.0f; isJumping = false; };


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
        Gizmos.DrawSphere(this.transform.position + translateSphereOnGround, 0.6f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(this.transform.position + translateSphereOnHight, 0.6f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(this.transform.position + new Vector3(0, -1, 0), 0.6f);

        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(this.transform.position + new Vector3(translateAttackCircle.x, translateAttackCircle.y, 0), 0.6f);
    }

    void calculatexSphereTranslate()
    {
        if (targetPos.x > 0) { translateSphereOnGround = new Vector3(xSphereTranslate, -1f, 0f); translateAttackCircle = new Vector2(1, 0); }
        else { translateSphereOnGround = new Vector3(-xSphereTranslate, -1f, 0f); translateAttackCircle = new Vector2(-1, 0); }
    }
    void calculateReverseXSphereTranslate()
    {
        if (targetPos.x > 0) { translateSphereOnGround = new Vector3(-xSphereTranslate, -1f, 0f); }
        else { translateSphereOnGround = new Vector3(xSphereTranslate, -1f, 0f); }
    }

    private void reverseMovement()
    {
        /// sprawdzenie 
        /// jesli rowny
        if (!isJumping)
        {
            calculateReverseXSphereTranslate();


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

                this.transform.position -= (this.targetPos * Time.deltaTime) * speed;
            }


        }

    } 
        // idzie x do gracza
        /// jesli mniejszy
        // sprawdzic czy platforma jest nad g�ow�
        //jesli nie dojsc do bezpiecznego
        //
        //jesli tak odejsc do punktu skoku
        public void slowMe(bool isSlow)
        {
            if (isSlow) {speed = Maxspeed / 5.0f;
                AttackpeedMultiply = MaxsAttackpeed * 5.0f;
            }
            else
            {
                speed = Maxspeed;
                AttackpeedMultiply = MaxsAttackpeed;
            }
        }

}
