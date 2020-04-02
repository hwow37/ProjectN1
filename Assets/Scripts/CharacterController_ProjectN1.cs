using UnityEngine;
using UnityEngine.Events;

public class CharacterController_ProjectN1 : MonoBehaviour
{
    public Rigidbody2D m_RigidbodyCharacter;

    // for checking Ground
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    private bool m_Grounded;
    const float k_GroundedRadius = .2f;

    // for setting right Dir
    private bool m_FacingRight = true;

    // for moving
    const float maxSpeed = 600f;

    // for shooting
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    private float bulletForce = 20f;

    // for attacking
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject attackEffect;
    [SerializeField] private GameObject attackEffect_L;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    private void Awake()
    {
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }


    // Check OnLand
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

    public void Jump(bool jump, float jumpPressure)
    {
        // If the player should jump
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            if (m_FacingRight)
            {
                if (m_RigidbodyCharacter.velocity.x > maxSpeed)
                {
                    m_RigidbodyCharacter.AddForce(new Vector2(-100f, jumpPressure));
                }
                else
                {
                    m_RigidbodyCharacter.AddForce(new Vector2(300f, jumpPressure));
                }
            }
            else
            {
                if (m_RigidbodyCharacter.velocity.x < -maxSpeed)
                {
                    m_RigidbodyCharacter.AddForce(new Vector2(100f, jumpPressure));
                }
                else
                {
                    m_RigidbodyCharacter.AddForce(new Vector2(-300f, jumpPressure));
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

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    public void Attack()
    {
        GameObject effect = Instantiate(attackEffect, attackPoint.position, attackPoint.rotation);
    }

    public void Attack_L()
    {
        GameObject effect = Instantiate(attackEffect_L, attackPoint.position, attackPoint.rotation);
    }
}