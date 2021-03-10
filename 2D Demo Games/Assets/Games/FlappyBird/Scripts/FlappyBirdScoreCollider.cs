using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdScoreCollider : MonoBehaviour
{
    GameObject scoreObject;

    private void Awake()
    {
        scoreObject = GameObject.FindGameObjectWithTag("Score");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("t");
        if (collision.transform.tag == "Player")
        {
            print("working");
            scoreObject.GetComponent<FlappyBirdScore>().score += 1;
        }
    }

}
