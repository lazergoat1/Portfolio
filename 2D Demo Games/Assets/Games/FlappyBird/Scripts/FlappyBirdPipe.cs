using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlappyBirdPipe : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
      if(collision.transform.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
