using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlappyBirdScore : MonoBehaviour
{
    public float score;
    public TextMeshProUGUI scoreText;

    private void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = score.ToString("0");
    }
}
