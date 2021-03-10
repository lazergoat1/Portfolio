using Cinemachine;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public GameObject middlePoint;
    public Camera cam;

    public float maxDistance;

    Vector2 mousePosition;

    private void Update()
    {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            
        if((mousePosition - (Vector2)transform.position).magnitude * 0.5 >= maxDistance)
        {
            print(1);
            middlePoint.transform.position = ((mousePosition - (Vector2)middlePoint.transform.position).normalized * maxDistance) + (Vector2)transform.position;
        }
        else
        {
            print("2");
            middlePoint.transform.position = (((Vector2)transform.position + mousePosition) / 2f);
        }
    }
}