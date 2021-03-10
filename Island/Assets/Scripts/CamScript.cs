using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CamScript : MonoBehaviour
{
    public Camera camera;
    public GameObject camParent;
    public GameObject player;

    public KeyCode rotateLeft;
    public KeyCode rotateRight;

    public float rotationAmount;

    private void Update()
    {
        MoveCam();
    }

    void MoveCam()
    {
        camParent.transform.position = player.transform.position;

        if (Input.GetKeyDown(rotateRight))
        {
            transform.rotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        else if(Input.GetKeyDown(rotateLeft))
        {
            transform.rotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
    }
}
