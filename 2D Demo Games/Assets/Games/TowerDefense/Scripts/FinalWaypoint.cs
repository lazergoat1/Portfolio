using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalWaypoint : MonoBehaviour
{
    public GameController gameController;
    public Vector3 boxSize;


    void collision()
    {
        Collider2D[] objectsInRange = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);

        foreach(Collider2D collider in objectsInRange)
        {
            if (collider.tag == "Enemy")
            {
                gameController.health -= collider.gameObject.GetComponent<EnemyController>().enemy.dmg;
                print(gameController.health);
                Destroy(collider.gameObject);

                if (gameController.health <= 0f)
                {
                    //You should just call the enemy kill function here
                    death();
                }
            }
        }
    }

    private void Update()
    {
        collision();
    }

    void death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, boxSize);
    }
}
