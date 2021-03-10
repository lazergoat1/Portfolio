using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceSpawnLevel : MonoBehaviour
{
    public Transform Player;
    public Transform lastSpawnPoint;
    public GameObject[] sections;
    public float spawnSeparation;

    public float maxSpawnDistance;

    private void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        float distance = (Player.position - lastSpawnPoint.position).magnitude;
        if (lastSpawnPoint != null && distance <= maxSpawnDistance)
        {
            GameObject newObject = Instantiate(sections[Random.Range(0,sections.Length)], lastSpawnPoint.position + new Vector3(0f,spawnSeparation), Quaternion.identity);
            lastSpawnPoint = newObject.transform;
        }
    }
}
