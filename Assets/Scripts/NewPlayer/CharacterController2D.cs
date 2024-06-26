using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_CeilingCheck;
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator legsAnimator;
    [SerializeField] public GameObject myPrefab;
    [SerializeField] public GameObject playerSmuga;
    [SerializeField] public GameObject slowTime;
    private GameController gameController;
    const float k_GroundedRadius = .2f;
    public bool m_Grounded;
    const float k_CeilingRadius = .2f;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;

    private float durationtoSpawn = 35.0f;
    private float spawner = 5.0f;
    private bool firstTouch =false;
    private float velocityX = 0;
    public float horizontalMove = 0f;

    public float jumpAnticipationFactor = 0.05f;
    private float jumpAnticipationTime = 0f;
    private bool isAnticipating = false;

    public float jumpWaitFactor = 0.5f;
    private float jumpWaitTime = 0f;
    private bool canJump = true;

    public float dashWaitFactor = 0.5f;
    private float dashWaitTime = 0f;
    private bool canDash = true;

    public bool isPlayerDead = false;

    public bool canUseSlow =false;

    private float jumpForce;
    [SerializeField]private float smugaSpawnerTimer=2.0f;
    private float defaulGravityForce;

    public float runSpeed = 40f;
    [SerializeField] private float m_MaxSpeed = 10f; // Maksymalna pr�dko�� biegu
    [SerializeField] private float m_Acceleration = 20f; // Przyspieszenie postaci

    [Header("Events")]
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        //bodyAnimator.

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }
    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        defaulGravityForce = GetComponent<Rigidbody2D>().gravityScale;
        jumpForce = m_JumpForce;
    }
    private void Update()
    {
        if (!isPlayerDead)
        {
            spawnSlowTimer();
            if (Input.GetButtonDown("SlowTime") && canUseSlow)
            {
                gameController.setSlow();
                canUseSlow = false;
            }

            smugaSpawnerTimer -= Time.deltaTime;
            if(gameController.isSlowTime && smugaSpawnerTimer<0.0f)
            {
                if(velocityX>0.0f) {smugaSpawnerTimer = 0.1f;Instantiate(playerSmuga, this.transform.position, Quaternion.identity);}
                
                else if(velocityX<0.0f) {smugaSpawnerTimer = 0.1f;Instantiate(playerSmuga, this.transform.position, Quaternion.Euler(0f, 180f, 0f));}
                
            }
            
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            velocityX = horizontalMove;
            if (horizontalMove == 0)
            {
                bodyAnimator.SetBool("isRunning", false);
                legsAnimator.SetBool("isRunning", false);
            }
            if (canJump && (Input.GetAxisRaw("Vertical") > 0.9f || Input.GetButtonDown("Jump")))
            {
                canJump = false;
                if (!m_Grounded)
                {
                    jumpAnticipationTime = 0;
                    isAnticipating = true;
                }

                Move(gameController.playerSpeedMultiplier * horizontalMove * Time.fixedDeltaTime, false, true);
            }
            if (!canJump)
            {
                jumpWaitTime += Time.deltaTime;
                if (jumpWaitTime >= jumpWaitFactor / gameController.playerSpeedMultiplier)
                {
                    canJump = true;
                    jumpWaitTime = 0;
                }
            }

            if (isAnticipating)
            {
                jumpAnticipationTime += Time.deltaTime;
                if (jumpAnticipationTime >= jumpAnticipationFactor)
                {
                    isAnticipating = false;
                }
            }
            if (!canDash)
            {
                dashWaitTime += Time.deltaTime;
                if (dashWaitTime >= dashWaitFactor / gameController.playerSpeedMultiplier)
                {
                    canDash = true;
                    dashWaitTime = 0;
                }
            }
            if (transform.position.y < 2)
            {
                transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
            }
            if (Input.GetButtonDown("Fire3") && canDash)
            {
                //dash here
                if (transform.localScale.x > 0)
                {
                    //transform.Translate(new Vector3(4,0,0));
                    m_Rigidbody2D.AddForce(new Vector2(2000f * gameController.playerSpeedMultiplier, 0f));
                }
                else
                {
                    //transform.Translate(new Vector3(-4, 0, 0));
                    m_Rigidbody2D.AddForce(new Vector2(-2000f * gameController.playerSpeedMultiplier, 0f));
                }
                bodyAnimator.SetTrigger("dash");
                legsAnimator.SetTrigger("dash");
                GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>().PlayPlayerDashSoundClip();
                canDash = false;
            }
        }
        else
        {
            Move(0, false, false);
        }
    }
    private void FixedUpdate()
    {
        if (!isPlayerDead)
        {
            Move(gameController.playerSpeedMultiplier * horizontalMove * Time.fixedDeltaTime, false, false);
            bool wasGrounded = m_Grounded;
            m_Grounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (!firstTouch) { 
                 firstTouch =true;
                }
                else { if (colliders[i].CompareTag("water")) Instantiate(myPrefab, this.transform.position, Quaternion.identity); }
                

                if (colliders[i].gameObject != gameObject)
                {
                    
                    m_Grounded = true;

                    if (!wasGrounded)
                    {
                        OnLandEvent.Invoke();
                        //bodyAnimator.SetBool("isJumping", false);
                        if (isAnticipating)
                        {
                            m_Grounded = false;
                            m_JumpForce = jumpForce * gameController.playerSpeedMultiplier;
                            m_Rigidbody2D.gravityScale = defaulGravityForce * gameController.playerSpeedMultiplier * gameController.playerSpeedMultiplier;
                            GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>().PlayPlayerJumpSoundClip();
                            bodyAnimator.SetBool("isJumping", true);
                            legsAnimator.SetBool("isJumping", true);
                            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
                            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                            ResetAnticipation();
                        }
                    }
                }

            }
           
           
        }
        else
        {
           
            Move(0, false, false);
        }

    }
    public void ResetAnticipation()
    {
        isAnticipating = false;
    }

    public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            bodyAnimator.SetBool("isRunning", true);
            legsAnimator.SetBool("isRunning", true);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		if(!m_Grounded && jump)
        {
            crouch = true;
        }

        if (m_Grounded || m_AirControl)
        {
            // Przyspieszenie i wyhamowanie postaci
            float targetSpeed = move * m_MaxSpeed;
            float speedDif = targetSpeed - m_Rigidbody2D.velocity.x;
            // Zmodyfikuj warto�� przyspieszenia w zale�no�ci od kierunku zmiany pr�dko�ci
            float accelRate;
            if (Mathf.Abs(targetSpeed) > 0.01f)
            {
                // Przyspieszanie
                accelRate = m_Acceleration;
            }
            else
            {
                // Zwalnianie - u�yj wi�kszej warto�ci dla szybszego zatrzymywania
                accelRate = m_Acceleration * 2f; // Mo�na dostosowa�, aby zwalnianie by�o szybsze
            }

            Vector3 targetVelocity = new Vector2(m_Rigidbody2D.velocity.x + speedDif * accelRate * Time.fixedDeltaTime, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip();
            }
        }
        if(m_Grounded)
        {
            bodyAnimator.SetBool("isJumping", false);
            legsAnimator.SetBool("isJumping", false);
        }

        if (m_Grounded && jump)
        {
            m_JumpForce = jumpForce * gameController.playerSpeedMultiplier;
            m_Rigidbody2D.gravityScale = defaulGravityForce * gameController.playerSpeedMultiplier * gameController.playerSpeedMultiplier;
            m_Grounded = false;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>().PlayPlayerJumpSoundClip();
            bodyAnimator.SetBool("isJumping", true);
            legsAnimator.SetBool("isJumping", true);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
	}

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void spawnSlowTimer()
    {
        spawner -= Time.deltaTime;
        if (spawner < 0.0f && canUseSlow==false)
        {
            float x = Random.Range(-60.0f, 60.0f);
            float y = Random.Range(3.0f, 13.0f);
            spawner = durationtoSpawn;
            Instantiate(slowTime, new Vector3(x,y,0), Quaternion.identity);
        }
    }

    public void pickUpSlow()
    {
        canUseSlow = true;
    }
}
