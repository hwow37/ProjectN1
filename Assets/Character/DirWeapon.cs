using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirWeapon : MonoBehaviour
{
    public float moveSpeed = 3f;
    public GameObject target;

    //public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        Vector3 lookDir = (Vector3)mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        lookDir.z = 0;
        transform.Rotate(target.transform.position);
        //transform.rotation = angle;
        //transform.LookAt(target.transform);
        //rb.rotation = angle;
    }
}
