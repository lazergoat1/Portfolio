using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDPlayer : MonoBehaviour
{
    public Rigidbody2D rb;

    public Camera cam;

    public Transform shootPoint;
    public GameObject bulletPrefab;

    public float jumpForce;
    public float moveSpeed;
    public float shootPower;
    public float smoothTime;

    private float smoothSpeed;

    public KeyCode jumpKey;
    public KeyCode shoot;

    private bool jump;

    private void FixedUpdate()
    {
        Jump();
        Move();
    }

    private void Update()
    {
        Shoot();
        JumpInput();
        Rotate();
    }

    private bool isGrounded()
    {
        LayerMask mask = LayerMask.GetMask("Terrain");

        if (Physics2D.Raycast(transform.position, Vector3.down, 0.8f, mask))
        {
            return true;
        }
        return false;
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rb.velocity.y);
    }

    void JumpInput()
    {
        if(Input.GetKeyDown(jumpKey) && isGrounded())
        {
            jump = true;
        }
    }
    private void Jump()
    {
        if(jump)
        {
            jump = false;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    private void Rotate()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = mousePos - rb.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        float smoothAngle = Mathf.SmoothDampAngle(rb.rotation, angle, ref smoothSpeed, smoothTime);
        rb.rotation = smoothAngle;
    }

    private void Shoot()
    {
        if(Input.GetKeyDown(shoot))
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.transform.position, Quaternion.identity);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            bulletRb.AddForce(shootPoint.up * shootPower, ForceMode2D.Impulse);
        }
    }
}
