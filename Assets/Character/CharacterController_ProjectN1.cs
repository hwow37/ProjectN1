using UnityEngine;
using UnityEngine.Events;

public class CharacterController_ProjectN1 : MonoBehaviour
{
    public Rigidbody2D m_RigidbodyCharacter;
    
    // for checking Ground
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    const float k_GroundedRadius = .2f;
    private bool m_Grounded;

    // for setting right Dir
    private bool m_FacingRight = true;

    // for moving
    const float maxSpeed = 600f;

    // for jumping
    private float m_JumpForce = 400f;

    // don't need
    //[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    //[SerializeField] private Transform m_CeilingCheck;
    //const float k_CeilingRadius = .2f;
    //private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    private void Awake()
    {
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
                    OnLandEvent.Invoke();
            }
        }
    }

    public void Move(float move, bool crouch, bool jump, float jumpPressure)
    {
        m_JumpForce = jumpPressure;
        // If crouching, check to see if the character can stand up
        /*if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}*/

        //only control the player if grounded or airControl is turned on
        /*if (m_Grounded)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, m_RigidbodyCharacter.velocity.y);
            m_RigidbodyCharacter.velocity = Vector3.SmoothDamp(m_RigidbodyCharacter.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }*/


        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            if (m_FacingRight)
            {
                if (m_RigidbodyCharacter.velocity.x > maxSpeed)
                {
                    m_RigidbodyCharacter.AddForce(new Vector2(-100f, m_JumpForce));
                }
                else
                {
                    m_RigidbodyCharacter.AddForce(new Vector2(300f, m_JumpForce));
                }
            }
            else
            {
                if (m_RigidbodyCharacter.velocity.x < -maxSpeed)
                {
                    m_RigidbodyCharacter.AddForce(new Vector2(100f, m_JumpForce));
                }
                else
                {
                    m_RigidbodyCharacter.AddForce(new Vector2(-300f, m_JumpForce));
                }
            }
        }
    }

    public void FlipFace()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = m_RigidbodyCharacter.transform.localScale;
        theScale.x *= -1;
        m_RigidbodyCharacter.transform.localScale = theScale;
    }
}