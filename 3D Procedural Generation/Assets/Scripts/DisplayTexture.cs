using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTexture : MonoBehaviour
{
    public RawImage image;
    public Transform player;
    public MapValues mapValues;

    private void Update()
    {
        float[,] noiseMap = Noise.GenerateNoise(mapValues.width, mapValues.height, mapValues.seed, mapValues.offsetX, mapValues.offsetY, mapValues.scale, mapValues.octaves, mapValues.persistance, mapValues.lacunarity);

        image.texture = GenerateTexture(noiseMap);
    }

    private Texture2D GenerateTexture(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Vector2 center = new Vector2(width * 0.5f, height * 0.5f);
        float[,] gradientMap = RadialGradient.GenerateGradient(mapValues.width, mapValues.height, mapValues.gradientThreshold, center, mapValues.gradientIntensityPoint, mapValues.gradientIntensity);

        Texture2D texture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y] + gradientMap[x, y]);
            }
        }

        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }
}
