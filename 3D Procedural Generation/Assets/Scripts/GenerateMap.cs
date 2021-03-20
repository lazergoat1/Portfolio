using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    public GameObject parent;
    public GameObject prefab;

    public MapValues mapValues;

    public float noise;
 
    public bool updateEverySeconds;
    public float updateDelay;
    float delay = 0;

    private void Start()
    {
      Generate();
    }

    private void Update()
    {
        delay -= Time.deltaTime;

        if(updateEverySeconds)
        {
            if(delay <= 0)
            {
                Generate();
                delay = updateDelay;
            }
        }
    }

    private void Generate()
    {
        foreach (Transform child in parent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (mapValues.scale <= 0)
        {
            mapValues.scale = 0.001f;
        }

        float[,] noiseMap = Noise.GenerateNoise(mapValues.width, mapValues.height, mapValues.seed, mapValues.offsetX, mapValues.offsetY, mapValues.scale, mapValues.octaves, mapValues.persistance, mapValues.lacunarity);

        for (int x = 0; x < mapValues.width; x++)
        {
            for (int y = 0; y < mapValues.height; y++)
            {
                noise = noiseMap[x, y];

                GameObject newCube = Instantiate(prefab, new Vector3(x, noise * mapValues.sizeMultiplier, y), Quaternion.identity);

                Color color = Color.Lerp(Color.black, Color.white, noise);
                newCube.gameObject.GetComponent<Renderer>().material.color = color;
                newCube.transform.parent = parent.transform;
            }
        }
    }

    private void OnValidate()
    {
        if(mapValues.octaves <= 0)
        {
            mapValues.octaves = 1;
        }

        if (mapValues.width < 1)
        {
            mapValues.width = 1;
        }
        if (mapValues.height < 1)
        {
            mapValues.height = 1;
        }
    }

}
