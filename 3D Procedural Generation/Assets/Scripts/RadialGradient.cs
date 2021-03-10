using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialGradient : MonoBehaviour
{
    //TEXTURE SETTINGS
    public int texWidth = 512;
    public int texHeight = 512;
 
    //MASK SETTINGS
    public float maskThreshold = 2.0f;
 
    //REFERENCES
    public Texture2D mask;
 
/*    private void Start()
    {
    GenerateTexture();
    }

    void GenerateTexture()
    {
        mask = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, true);

        Vector2 maskCenter = new Vector2(texWidth * 0.5f, texHeight * 0.5f);

        for (int y = 0; y < mask.height; ++y)
        {
            for (int x = 0; x < mask.width; ++x)
            {

                float distFromCenter = Vector2.Distance(maskCenter, new Vector2(x, y));

                float currentAlpha = 1;

                if ((1 - (distFromCenter / texWidth)) >= 0)
                {
                    currentAlpha = (1 - (DistanceFromCenter / size));
                }
                else
                {
                    currentAlpha = 0;
                }

                Color color = new Color(currentAlpha, currentAlpha, currentAlpha, currentAlpha);
                texture.SetPixel(x, y, color);
            }
        }
        mask.Apply();
    }*/
}
