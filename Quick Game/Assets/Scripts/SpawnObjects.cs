using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject player;
    GameObject lastSpawnObject;
    float distance;

    public float spawnDelay = 5f;
    public float lastPosition = -13f;
    public float spawnSpace;
    public float xOffset;
    public float zOffset;

    void Start()
    {
        StartCoroutine(Spawn(spawnDelay));
    }

    private void Update()
    {
        if (lastSpawnObject != null)
        {
            distance = (lastSpawnObject.transform.position - player.transform.position).magnitude;
        }
    }

    IEnumerator Spawn(float waitTime)
    {
        while(true)
        {
            if (distance < 100f)
            {
                GameObject newObject = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
                newObject.transform.position = new Vector3(xOffset, lastPosition + spawnSpace, zOffset);
                newObject.transform.parent = gameObject.transform;
                lastPosition = lastPosition + spawnSpace;
                lastSpawnObject = newObject;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}
