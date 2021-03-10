using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceCamera : MonoBehaviour
{
    public Camera cam;
    public Transform player;

    private void Update()
    {
        SetCamPosition();
    }
    void SetCamPosition()
    {
        cam.transform.position = new Vector3(cam.transform.position.x, player.position.y, cam.transform.position.z);
    }
}
