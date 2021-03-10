using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayer : MonoBehaviour
{
    public CharacterController characterController;
    public Camera cam;
    public Camera secondCam;
    public Transform playerBody;
    public Transform groundCheck;
    public Animator animator;

    public KeyCode jumpKey;
    public KeyCode attackKey;
    public KeyCode sprint;

    public float turnSmoothTime;
    public float moveSmoothTime;
    public float mouseSensitivity;

    public float animationSpeed;
    public float moveSpeed;
    public float runningMultiplier;
    float runningAnimationSpeed;
    float runningSpeed;
    float privateAnimationSpeed;
    float privateMoveSpeed;

    public float gravityScale;
    public float jumpForce;
    public float groundCheckRadius;

    float xRotation = 0f;
    float yRotation = 0f;
    float currentSpeed;

    Vector3 velocity;

    bool isGrounded;
    bool jump;
    bool isRunning;

    float moveSmoothVelocity;

    private void Start()
    {
        secondCam.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Jump();
        Attack();
        Run();
    }

    private void FixedUpdate()
    {
        Gravity();
        Rotate();
        Move();
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z).normalized;

        if(move.magnitude >= 0.1 && animator.GetBool("IsAttacking") == false)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, privateMoveSpeed, ref moveSmoothVelocity, moveSmoothTime);
            characterController.Move(move * currentSpeed * Time.deltaTime);

            animator.SetFloat("Speed", ((currentSpeed - 0f) / (privateMoveSpeed - 0f)));
            animator.speed = (((currentSpeed - 0f) / (privateMoveSpeed - 0f)) * privateAnimationSpeed);
        }
        else
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, 0f, ref moveSmoothVelocity, moveSmoothTime);
            animator.speed = privateAnimationSpeed;
            animator.SetFloat("Speed", ((currentSpeed - 0f) / (privateMoveSpeed - 0f)));
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            jump = true;
        }
    }

    void Gravity()
    {
        float gravity = -9.81f * gravityScale;
        Collider[] ground = Physics.OverlapSphere(groundCheck.position, groundCheckRadius);

        foreach(Collider collider in ground)
        {
            if(collider.tag != "Player")
            {
                isGrounded = true;
                break;
            }
            else
            {
                isGrounded = false;
            }
        }

        if (jump == true)
        {
            velocity.y += Mathf.Sqrt(jumpForce * -2 * gravity);
            jump = false;
        }

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void Rotate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
        secondCam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void Attack()
    {
        if (Input.GetKeyDown(attackKey) && animator.GetBool("IsAttacking") == false)
        {
            animator.SetTrigger("Attack");

            //deal dmg to anything within the attack range
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetBool("IsAttacking", true);
        }
        else
        {
            animator.SetBool("IsAttacking", false);
        }
    }

    void Run()
    {
        runningSpeed = moveSpeed * runningMultiplier;
        runningAnimationSpeed = animationSpeed * runningMultiplier;

        if (Input.GetKey(sprint) && animator.GetBool("IsAttacking") == false)
        {
            privateMoveSpeed = runningSpeed;
            privateAnimationSpeed = runningAnimationSpeed;
            isRunning = true;
        }
        else
        {
            privateMoveSpeed = moveSpeed;
            privateAnimationSpeed = animationSpeed;
            isRunning = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
