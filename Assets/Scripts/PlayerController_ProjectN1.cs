using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_ProjectN1 : MonoBehaviour
{
    public Animator playerAnim;
    public GameObject player;
    public GameObject playerWeapon;

    // for checking Ground
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_FireCheck;
    [SerializeField] private LayerMask m_WhatIsGround;
    const float k_GroundedRadius = .1f;
    const float k_FireRadius = .1f;
    private bool m_Grounded = true;

    // for setting right Dir
    [SerializeField] private Camera cam;
    private Vector3 temp_theScale;
    private Vector2 mousePos;
    private bool isRight = true;

    // for jumping
    [SerializeField] private JumpBar jumpBar;
    private float jumpPressure = 100f;
    private float jumpRate = 0.5f;
    private float nextJump = 0.0f;
    private bool isCrouching = false;
    const float minJumpPressure = 150f;
    const float maxJumpPressure = 1000f;

    // for shooting
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    private float bulletForce = 20f;
    private float angle;
    private float x = 0, y = 0;
    private float fireRate = 0.5f;
    private float nextFire = 0.0f;
    private float attackRate = 0.5f;
    private float nextAttack = 0.0f;

    // for attacking
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject attackEffect;
    [SerializeField] private GameObject attackEffect_L;

    // for Setting UI and Menu
    public GameObject playerUI;
    public TravelMenu travelMenu;

    private void Start()
    {
        temp_theScale = player.transform.localScale;
    }

    void Update()
    {
        // Travel in RestArea
        if (Input.GetButtonDown("Interact") && !PauseMenu.GameIsPaused && RestArea.inRestArea && !TravelMenu.inTravelMenu)
        {
            playerUI.SetActive(false);
            travelMenu.SetTravelMenu(true);
        }

        // Crouch
        if (Input.GetButton("Crouch") && Physics2D.OverlapCircle(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround)
            && Time.time > nextJump && !PauseMenu.GameIsPaused)
        {
            isCrouching = true;
            playerAnim.SetBool("IsCrouching", true);
            if (jumpPressure < maxJumpPressure)
            {
                jumpPressure += Time.deltaTime * 700f;
            }
            else
            {
                jumpPressure = maxJumpPressure;
            }
            jumpBar.SetJump(jumpPressure);
        }
        else if (Input.GetButtonUp("Crouch") && Physics2D.OverlapCircle(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround)
                 && isCrouching && !PauseMenu.GameIsPaused)
        {
            if (jumpPressure < minJumpPressure)
            {
                jumpPressure = minJumpPressure;
            }

            nextJump = Time.time + jumpRate;

            Jump(jumpPressure);
            isCrouching = false;

            playerAnim.SetBool("IsJumping", true);
            playerAnim.SetBool("IsCrouching", false);
        }

        // Check Falling
        if (playerWeapon.GetComponent<Rigidbody2D>().velocity.y < -0.1)
        {
            m_Grounded = false;
            playerAnim.SetBool("IsFalling", true);
        }

        // Face Mouse
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x - playerWeapon.GetComponent<Rigidbody2D>().position.x < 0)
        {
            isRight = false;
        }
        else if (mousePos.x - playerWeapon.GetComponent<Rigidbody2D>().position.x >= 0)
        {
            isRight = true;
        }

        // Shooting Recoil
        if (Input.GetButtonDown("FireBullet") && Time.time > nextFire && !Physics2D.OverlapCircle(m_FireCheck.position, k_FireRadius, m_WhatIsGround)
            && !isCrouching && !PauseMenu.GameIsPaused)
        {
            Shoot();
            nextFire = Time.time + fireRate;

            if (angle > 0)
            {
                // 1 quadrant
                if (angle < 90)
                {
                    x = 90 - angle;
                    y = angle;
                    player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2f * x, -2f * y));
                }
                // 2 quadrant
                else
                {
                    x = angle - 90;
                    y = 180 - angle;
                    player.GetComponent<Rigidbody2D>().AddForce(new Vector2(2f * x, -2f * y));
                }
            }
            else
            {
                // 3 quadrant
                if (angle < -90)
                {
                    x = angle + 90;
                    y = 180 + angle;
                    player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2f * x, 2f * y));
                }
                // 4 quadrant
                else
                {
                    x = 90 + angle;
                    y = angle;
                    player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2f * x, -2f * y));
                }
            }
        }

        // Attack
        if (Input.GetButtonDown("AttackWeapon") && Time.time > nextAttack && !isCrouching && !PauseMenu.GameIsPaused)
        {
            // Rigth direction
            nextAttack = Time.time + attackRate;
            float xDir = Mathf.Abs(angle);
            if (xDir < 90)
            {
                playerWeapon.GetComponent<Rigidbody2D>().AddForce(new Vector2((90 - xDir) * 2f, 0f));
                Attack();
            }
            // Left direction
            else
            {
                playerWeapon.GetComponent<Rigidbody2D>().AddForce(new Vector2(-(xDir - 90) * 2f, 0f));
                Attack_L();
            }
        }
    }

    void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if (!m_Grounded)
                {
                    m_Grounded = true;
                    OnLanding();
                }
            }
        }

        SetFace();
        faceMouse();
    }

    public void OnLanding()
    {
        jumpPressure = 0f;
        playerAnim.SetBool("IsJumping", false);
        playerAnim.SetBool("IsFalling", false);
        Vector2 position = player.transform.position;
        position.y -= 0.161f;
        playerWeapon.transform.position = position;
    }

    void faceMouse()
    {
        Vector3 lookDir = mousePos - playerWeapon.GetComponent<Rigidbody2D>().position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        playerWeapon.GetComponent<Rigidbody2D>().rotation = angle;
    }

    public void Jump(float jumpPressure)
    {
        if (isRight)
        {
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(300f, jumpPressure));
        }
        else
        {
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-300f, jumpPressure));
        }
    }

    public void SetFace()
    {
        Vector3 theScale = player.transform.localScale;
        if (!isRight)
        {
            theScale.x = -temp_theScale.x;
        }
        else
        {
            theScale.x = temp_theScale.x;
        }
        player.transform.localScale = theScale;
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