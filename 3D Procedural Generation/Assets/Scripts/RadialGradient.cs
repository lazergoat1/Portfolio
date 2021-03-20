using UnityEngine;

public class RadialGradient : MonoBehaviour
{
    public static float[,] GenerateGradient(int width, int height, float gradientThreshold, Vector2 center, float intencityPoint, float intensity)
    {
        float[,] values = new float[width, height];

        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                float DistanceFromCenter = Vector2.Distance(center, new Vector2(x, y));
                float currentAlpha = 1;

                if (DistanceFromCenter / width >= 0)
                {
                    if(DistanceFromCenter <= intencityPoint)
                    {
                        currentAlpha = (gradientThreshold - (DistanceFromCenter / width) + (intensity - DistanceFromCenter / width));
                    }
                    else
                    {
                        currentAlpha = (gradientThreshold - (DistanceFromCenter / width));
                    }
                }
                else
                {
                    currentAlpha = 0;
                }

                values[x, y] = currentAlpha;

            }
        }
        return values;
    }
}
