using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_ProjectN1 : MonoBehaviour
{
    public CharacterController_ProjectN1 controller;
    public Animator playerAnim;
    public Rigidbody2D m_RigidbodyWeapon;
    public Player player;
    public Player playerWeapon;

    // for checking Ground
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_FireCheck;
    [SerializeField] private LayerMask m_WhatIsGround;
    const float k_GroundedRadius = .1f;
    const float k_FireRadius = .1f;

    // for setting right Dir
    [SerializeField] private Camera cam;
    private Vector2 mousePos;
    private bool m_flip = false;
    private bool isRight = true;

    // for jumping
    public JumpBar jumpBar;
    private float jumpPressure = 100f;
    private float jumpRate = 0.5f;
    private float nextJump = 0.0f;
    private bool jump = false;
    private bool isCrouching = false;
    const float minJumpPressure = 150f;
    const float maxJumpPressure = 1000f;

    // for shooting
    private float angle;
    private float x = 0, y = 0;
    private float fireRate = 0.5f;
    private float nextFire = 0.0f;
    private float attackRate = 0.5f;
    private float nextAttack = 0.0f;

    void Update()
    {
        if (Input.GetButtonDown("Load") && !PauseMenu.GameIsPaused) {
            Debug.Log("Loaded");
            PlayerData data = SaveSystem.LoadPlayer();

            Vector2 position;
            position.x = data.position[0];
            position.y = data.position[1];

            player.transform.position = position;
            position.y = data.position[1] - 0.161f;
            playerWeapon.transform.position = position;
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
            jump = true;
            isCrouching = false;
            playerAnim.SetBool("IsCrouching", false);
        }

        // Check Falling
        if (m_RigidbodyWeapon.velocity.y < -0.1)
        {
            playerAnim.SetBool("IsFalling", true);
        }
        else
        {
            playerAnim.SetBool("IsFalling", false);
        }

        // Face Mouse
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x - m_RigidbodyWeapon.position.x < 0 && isRight)
        {
            m_flip = true;
            isRight = false;
        }
        else if (mousePos.x - m_RigidbodyWeapon.position.x >= 0 && !isRight)
        {
            m_flip = true;
            isRight = true;
        }

        // Shooting Recoil
        if (Input.GetButtonDown("FireBullet") && Time.time > nextFire && !Physics2D.OverlapCircle(m_FireCheck.position, k_FireRadius, m_WhatIsGround)
            && !isCrouching && !PauseMenu.GameIsPaused)
        {
            controller.Shoot();
            nextFire = Time.time + fireRate;

            if (angle > 0)
            {
                // 1 quadrant
                if (angle < 90)
                {
                    x = 90 - angle;
                    y = angle;
                    m_RigidbodyWeapon.AddForce(new Vector2(-2f * x, -2f * y));
                }
                // 2 quadrant
                else
                {
                    x = angle - 90;
                    y = 180 - angle;
                    m_RigidbodyWeapon.AddForce(new Vector2(2f * x, -2f * y));
                }
            }
            else
            {
                // 3 quadrant
                if (angle < -90)
                {
                    x = angle + 90;
                    y = 180 + angle;
                    m_RigidbodyWeapon.AddForce(new Vector2(-2f * x, 2f * y));
                }
                // 4 quadrant
                else
                {
                    x = 90 + angle;
                    y = angle;
                    m_RigidbodyWeapon.AddForce(new Vector2(-2f * x, -2f * y));
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
                m_RigidbodyWeapon.AddForce(new Vector2((90 - xDir) * 2f, 0f));
                controller.Attack();
            }
            // Left direction
            else
            {
                m_RigidbodyWeapon.AddForce(new Vector2(-(xDir - 90) * 2f, 0f));
                controller.Attack_L();
            }
        }
    }

    void FixedUpdate()
    {
        controller.Jump(jump, jumpPressure);
        if (m_flip)
        {
            controller.FlipFace();
            m_flip = false;
        }
        jump = false;

        faceMouse();
    }

    public void OnLanding()
    {
        jumpPressure = 0f;

        playerAnim.SetBool("IsJumping", false);
        playerAnim.SetBool("IsFalling", false);
    }

    void faceMouse()
    {
        Vector3 lookDir = mousePos - m_RigidbodyWeapon.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        m_RigidbodyWeapon.rotation = angle;
    }
}