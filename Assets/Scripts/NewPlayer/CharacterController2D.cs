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

    [SerializeField] private float m_MaxSpeed = 10f; // Maksymalna prêdkoœæ biegu
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

    private void FixedUpdate()
    {
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
                }
            }
        }
    }

    public void Move(float move, bool crouch, bool jump)
    {
        if (!crouch && Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
        {
            crouch = true;
        }

        if (m_Grounded || m_AirControl)
        {
            // Przyspieszenie i wyhamowanie postaci
            float targetSpeed = move * m_MaxSpeed;
            float speedDif = targetSpeed - m_Rigidbody2D.velocity.x;
            // Zmodyfikuj wartoœæ przyspieszenia w zale¿noœci od kierunku zmiany prêdkoœci
            float accelRate;
            if (Mathf.Abs(targetSpeed) > 0.01f)
            {
                // Przyspieszanie
                accelRate = m_Acceleration;
            }
            else
            {
                // Zwalnianie - u¿yj wiêkszej wartoœci dla szybszego zatrzymywania
                accelRate = m_Acceleration * 2f; // Mo¿na dostosowaæ, aby zwalnianie by³o szybsze
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
