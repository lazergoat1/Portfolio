using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonPlayer : MonoBehaviour
{
    public CharacterController characterController;
    public Camera cam;
    public Transform playerBody;
    public Transform groundCheck;
    public Animator animator;

    public KeyCode jumpKey;
    public KeyCode attackKey;
    public KeyCode sprint;

    public float turnSmoothTime;
    public float moveSmoothTime;

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

    float currentSpeed;

    Vector3 velocity;

    bool isGrounded;
    bool jump;
    bool isRunning;

    float turnSmoothVelocity;
    float moveSmoothVelocity;


    private void Start()
    {
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
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f && animator.GetBool("IsAttacking") == false)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            currentSpeed = Mathf.SmoothDamp(currentSpeed, privateMoveSpeed, ref moveSmoothVelocity, moveSmoothTime);

            animator.SetFloat("Speed", ((currentSpeed - 0f) / (privateMoveSpeed - 0f)));
            animator.speed = (((currentSpeed - 0f) / (privateMoveSpeed - 0f)) * privateAnimationSpeed);

            characterController.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
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
        if (Input.GetKeyDown(jumpKey) && isGrounded && animator.GetBool("IsAttacking") == false)
        {
            jump = true;
        }
    }

    void Gravity()
    {
        
        float gravity = -9.81f * gravityScale;
        Collider[] ground = Physics.OverlapSphere(groundCheck.position, groundCheckRadius);
        

        foreach (Collider collider in ground)
        {
            if (collider.tag != "Player")
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

        if(Input.GetKey(sprint) && animator.GetBool("IsAttacking") == false)
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
