using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public Rigidbody2D rb;

    public KeyCode jumpKey;

    public float jumpPower;
    public float moveSpeed;

    private void FixedUpdate()
    {
        Move();
        Jump();
        isGrounded();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(moveSpeed * h, rb.velocity.y);
    }


    bool isGrounded()
    {
        LayerMask mask = LayerMask.GetMask("Platform");

        if (Physics2D.Raycast(transform.position, Vector3.down, 0.8f, mask))
        {
            return true;
        }
        return false;
    }

    void Jump()
    {
        if(isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);
        }
    }

}
