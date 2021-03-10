using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text score;
    public GameObject player;
    float highestY;

    void Update()
    {
        if(highestY <= player.transform.position.y)
        {
            score.text = player.transform.position.y.ToString("0");
            highestY = player.transform.position.y;
        }
        else
        {
            score.text = highestY.ToString("0");
        }
    }
}
