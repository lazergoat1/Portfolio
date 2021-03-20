using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPositioner : MonoBehaviour
{
    public MapValues map;
    public GenerateMap generateMap;

    private void Awake()
    {
        if (!generateMap.enabled)
        {
            this.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        transform.position = new Vector3(map.width / 2, map.width + map.height/2, map.height / 2); 
    }
}
