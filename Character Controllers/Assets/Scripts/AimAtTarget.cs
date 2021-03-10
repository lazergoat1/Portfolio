using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtTarget : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject moveObject;
    public GameObject grabTargetObject;

    private void Update()
    {
        moveObject.transform.position = targetObject.transform.position;
        grabTargetObject.transform.position = targetObject.transform.position;
    }
}
