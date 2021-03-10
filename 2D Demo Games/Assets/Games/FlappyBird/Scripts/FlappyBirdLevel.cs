using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdLevel : MonoBehaviour
{
    public Transform Player;
    public Transform lastSpawnPoint;
    public GameObject section;
    public float spawnSeparation;
    public int maxY;
    public int minY;
    public float maxSpawnDistance;

    private void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        float distance = (Player.position - lastSpawnPoint.position).magnitude;
        if(lastSpawnPoint != null && distance <= maxSpawnDistance)
        {
            Vector2 lastSpawnPointPosition = new Vector3(lastSpawnPoint.position.x, 0);
            GameObject newObject = Instantiate(section, lastSpawnPointPosition + new Vector2(spawnSeparation, Random.Range(minY, maxY)), Quaternion.identity);
            lastSpawnPoint = newObject.transform;
        }
    }

}
