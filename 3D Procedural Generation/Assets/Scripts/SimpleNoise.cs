using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoise : MonoBehaviour
{
    public static float[,] GenerateNoise(int width, int height, float scale, float startX, float startY)
    {
        float[,] noiseMap = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / scale + startX;
                float yCoord = (float)y / scale + startY;

                float perlinValue = Mathf.PerlinNoise(xCoord, yCoord);

                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }
}
