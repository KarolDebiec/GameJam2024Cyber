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

    const float k_GroundedRadius = .2f;
    public bool m_Grounded;
    const float k_CeilingRadius = .2f;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;

    public float runSpeed = 40f;
    private float horizontalMove = 0f;

    public float jumpAnticipationFactor = 0.05f;
    public float jumpAnticipationTime = 0f;
    public bool isAnticipating = false;


    [SerializeField] private float m_MaxSpeed = 10f; // Maksymalna pr�dko�� biegu
    [SerializeField] private float m_Acceleration = 20f; // Przyspieszenie postaci

    [Header("Events")]
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }
    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (Input.GetButtonDown("Jump"))
        {
            if(!m_Grounded)
            {
                jumpAnticipationTime = 0;
                isAnticipating = true;
            }
            Move(horizontalMove * Time.fixedDeltaTime, false, true);
        }
        if(isAnticipating)
        {
            jumpAnticipationTime += Time.deltaTime;
            if(jumpAnticipationTime >= jumpAnticipationFactor)
            {
                isAnticipating = false;
            }
        }
    }
    private void FixedUpdate()
    {
        Move(horizontalMove * Time.fixedDeltaTime, false, false);
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                {
                    gameObject.GetComponent<PlayerMovement>().OnLanding();
                    OnLandEvent.Invoke();
                    if (isAnticipating)
                    {
                        Debug.Log("on grounded : " + m_JumpForce);
                        m_Grounded = false;
                        m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
                        m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                        ResetAnticipation();
                    }
                }
            }
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

        if (m_Grounded && jump)
        {
            m_Grounded = false;
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
}
