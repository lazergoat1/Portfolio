using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float movement = 10f;
    public float moveSpeed = 10f;
    public float grabRadius = 0.5f;

    public GameObject radiusGfx;
    public GameObject rotateObject;
    public GameObject currentRotateObject;

    public GameObject player;
    public Rigidbody2D rb;

    public SpringJoint2D rope;
    public LineRenderer ropeGFX;
    public LineRenderer targetRopeGFX;
    public LayerMask grabObject;
    public KeyCode grappleKey;
    Collider2D[] grabOptions;
    bool isGrabbing = true;

    private void FixedUpdate()
    {
        if (currentRotateObject != null)
        {
            SpinObject();
        }
    }

    void Update()
    {
        Grab();
        GetObject();
        DisplayRadius();
    }

    void DisplayRadius()
    {
        radiusGfx.transform.localScale = new Vector3(grabRadius, grabRadius, grabRadius)/67;
    }

    void Grab()
    {
        if(rotateObject != null)
        {
            if(currentRotateObject != null)
            {
                ropeGFX.enabled = true;
                ropeGFX.SetPosition(0, player.transform.position);
                ropeGFX.SetPosition(1, currentRotateObject.transform.position);
            }
            else if(currentRotateObject == null)
            {
                ropeGFX.enabled = false;
            }

            if (isGrabbing == false && Input.GetKeyDown(grappleKey))
            {
                FindObjectOfType<AudioManager>().Play("Attach");
                currentRotateObject = rotateObject;
                float ropeLength = Vector2.Distance(rotateObject.transform.position, player.transform.position);
                rope.enabled = true;
                rope.connectedAnchor = rotateObject.transform.position;
                rope.distance = ropeLength;
                isGrabbing = true;
            }
            else if(isGrabbing == true && Input.GetKeyDown(grappleKey))
            {
                FindObjectOfType<AudioManager>().Play("Detach");
                currentRotateObject = null;
                rope.enabled = false;
                isGrabbing = false;
            }
        }
    }

    void GetObject()
    {
        grabOptions = Physics2D.OverlapCircleAll(player.transform.position, grabRadius, grabObject);
        float minDistance = Mathf.Infinity;

        if (grabOptions.Length > 0)
        {
            foreach (Collider2D option in grabOptions)
            {
                float distance = (option.transform.position - transform.position).sqrMagnitude;
                if (distance < minDistance)
                {
                    rotateObject = option.gameObject;
                    minDistance = distance;
                }
            }
            if(rotateObject != currentRotateObject)
            {
                targetRopeGFX.enabled = true;
                targetRopeGFX.SetPosition(0, player.transform.position);
                targetRopeGFX.SetPosition(1, rotateObject.transform.position);
            }
            else
            {
                targetRopeGFX.enabled = false;
            }
        }
        else if (grabOptions.Length <= 0)
        {
            rotateObject = null;
            currentRotateObject = null;
            rope.enabled = false;
            ropeGFX.enabled = false;
            targetRopeGFX.enabled = false;
        }
       
    }

    public void SpinObject()
    {
        //Creates a thrust which is just the horizontal input 
        //Then adds a local force on the x of the object 
        float thrust = Input.GetAxis("Horizontal");
        rb.AddRelativeForce(Vector3.right * thrust * moveSpeed * Time.deltaTime);

        //Looks towards the rotate object
        Vector3 lookDir = currentRotateObject.transform.position - transform.position;
        //
        Vector3 playerPos = transform.position;
        playerPos.z = 1f;
        transform.LookAt(playerPos, lookDir);
    }
}
