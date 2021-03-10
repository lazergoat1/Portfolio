using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayer : MonoBehaviour
{
    public CharacterController characterController;
    public Camera cam;
    public Transform moveTowards;
    public Transform playerBody;
    public Transform groundCheck;

    public KeyCode interactKey;

    public float turnSmoothTime;
    public float moveSmoothTime;

    public float moveSpeed;

    public float gravityScale;
    public float jumpForce;
    public float groundCheckRadius;
    public float interactionCheckRadius;

    Vector3 velocity;

    bool isGrounded;
    bool movingWithKeys;
    bool objectFound;

    float turnSmoothVelocity;
    private void Update()
    {
        InteractMove();
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

        if (direction.magnitude >= 0.1f)
        {
            movingWithKeys = true;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        else
        {
            movingWithKeys = false;
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

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void InteractMove()
    {
        if(Input.GetKey(interactKey))
        {
            float distance = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(groundCheck.position, interactionCheckRadius);

            if(colliders.Length > 0)
            {
                foreach(Collider collider in colliders)
                {
                    if (collider.tag == "Interact")
                    {
                        objectFound = true;
                        float newDistance = (collider.transform.position - playerBody.position).magnitude;

                        if (newDistance < distance)
                        {
                            moveTowards = collider.transform;
                            distance = newDistance;
                        }
                    }

                    else if(collider == colliders[colliders.Length - 1] && objectFound == false)
                    {
                        moveTowards = null;
                    }
                }
                objectFound = false;

                if (moveTowards != null)
                {
                    if (movingWithKeys == false)
                    {

                        if (distance <= moveTowards.GetComponent<Pickup>().interactionCheckRadius)
                        {
                            moveTowards.GetComponent<Pickup>().playing = true;
                            moveTowards.GetComponent<Pickup>().PickupFunction(gameObject);
                        }

                        else
                        {
                            Vector3 direction = (moveTowards.position - playerBody.position).normalized;

                            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                            transform.rotation = Quaternion.Euler(0f, angle, 0f);

                            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                            characterController.Move(moveDir * moveSpeed * Time.deltaTime);
                        }
                    }
                    else
                    {
                        moveTowards.GetComponent<Pickup>().playing = false;
                        moveTowards.GetComponent<Pickup>().pickupTime = moveTowards.GetComponent<Pickup>().setPickupTime;
                    }
                }
                
            }
        }
        else if (moveTowards != null)
        {
            moveTowards.GetComponent<Pickup>().playing = false;
            moveTowards.GetComponent<Pickup>().pickupTime = moveTowards.GetComponent<Pickup>().setPickupTime;
            moveTowards = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, interactionCheckRadius);
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
