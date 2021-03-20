using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialGradientDisplay : MonoBehaviour
{
    public RawImage image;
    public MapValues mapValues;

    public int texWidth = 1000;
    public int texHeight = 1000;

    private void Update()
    {
        image.texture = GenerateTexture();
    }

    private Texture2D GenerateTexture()
    {
        Texture2D mask = new Texture2D(texWidth, texHeight);

        Vector2 center = new Vector2(texWidth * 0.5f, texHeight * 0.5f);
        float[,] gradient = RadialGradient.GenerateGradient(texWidth, texHeight, mapValues.gradientThreshold, center, mapValues.gradientIntensityPoint, mapValues.gradientIntensity);

        for (int y = 0; y < texWidth; ++y)
        {
            for (int x = 0; x < texHeight; ++x)
            {
                Color color = new Color(gradient[x, y], gradient[x, y], gradient[x, y], 1);
                mask.SetPixel(x, y, color);
            }
        }
        mask.Apply();

        return mask;
    }
}
