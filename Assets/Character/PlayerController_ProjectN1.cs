using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_ProjectN1 : MonoBehaviour
{
	public CharacterController_ProjectN1 controller;
	public Animator playerAnim;
	public Rigidbody2D m_RigidbodyWeapon;

	// for checking Ground
	[SerializeField] private Transform m_CeilingCheck;
	[SerializeField] private LayerMask m_WhatIsGround;
	const float k_CeilingRadius = .2f;

	// for setting right Dir
	[SerializeField] private Camera cam;
	private Vector2 mousePos;
	private bool m_flip = false;
	private bool isRight = true;

	// for moving
	[Range(0, 100f)] public float runSpeed = 40f;
	private float horizontalMove = 0f;

	// for jumping
	private bool jump = false;
	private float jumpPressure = 100f;
	const float minJumpPressure = 150f;
	const float maxJumpPressure = 1000f;

	void Update()
	{

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		
		if (Input.GetButton("Jump"))
		{
			if(jumpPressure < maxJumpPressure)
			{
				jumpPressure += Time.deltaTime * 500f;
			}
			else
			{
				jumpPressure = maxJumpPressure;
			}
		}else if (Input.GetButtonUp("Jump"))
		{
			if(jumpPressure < minJumpPressure)
			{
				jumpPressure = minJumpPressure;
			}

			jump = true;
			playerAnim.SetBool("IsJumping", true);
		}

		if (Input.GetButton("Crouch"))
		{
			playerAnim.SetBool("IsCrouching", true);
			if (jumpPressure < maxJumpPressure)
			{
				jumpPressure += Time.deltaTime * 500f;
			}
			else
			{
				jumpPressure = maxJumpPressure;
			}
		}
		else if (Input.GetButtonUp("Crouch"))
		{
			if (jumpPressure < minJumpPressure)
			{
				jumpPressure = minJumpPressure;
			}

			jump = true;
			playerAnim.SetBool("IsCrouching", false);
		}

		if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
		{
			playerAnim.SetBool("IsFalling", true);
		}

		if (m_RigidbodyWeapon.velocity.y < -0.1)
		{
			playerAnim.SetBool("IsFalling", true);
		}
		else
		{
			playerAnim.SetBool("IsFalling", false);
		}

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
	}


	void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump, jumpPressure);
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
		float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
		m_RigidbodyWeapon.rotation = angle;
	}
}