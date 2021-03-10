using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    public GameObject Tower;
    public GameObject RealTower;
    public GameController gameController;

    public float cost;

    Vector3 worldPosition;
    public void Place()
    {
        if (gameController.cash >= cost)
        {
            GameObject newTower = Instantiate(Tower, Input.mousePosition, Quaternion.identity);
            StartCoroutine(ChosePlacement(newTower));
        }
    }

    IEnumerator ChosePlacement(GameObject tower)
    {
        while(true)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            tower.transform.position = worldPosition;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                gameController.cash -= cost;
                Destroy(tower);
                Instantiate(RealTower, worldPosition, Quaternion.identity);
                StopAllCoroutines();
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Destroy(tower);
                StopAllCoroutines();
            }
            
            yield return new WaitForSeconds(0f);
        }
    }
}
