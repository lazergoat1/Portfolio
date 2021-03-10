using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdCamera : MonoBehaviour
{
    public Camera cam;
    public Transform player;

    private void Update()
    {
        SetCamPosition();
    }
    void SetCamPosition()
    {
        cam.transform.position = new Vector3(player.position.x, cam.transform.position.y, cam.transform.position.z);
    }
}
