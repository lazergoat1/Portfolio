using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWave : MonoBehaviour
{
    public GameController gameController;
    public GameObject baseEnemyPrefab;

    public void StartWave()
    {
        if(gameController.currentWave < gameController.waves.Length)
        {
            Spawn(gameController.waves[gameController.currentWave]);
        }
    }

    void Spawn(Wave wave)
    {
        for (int n = 0; n < wave.enemies.Count; n++)
        {
            StartCoroutine(spawnEnemyWithWait(wave.timeBetweenSpawn, n, gameController.waves[0]));
        }
    }

    IEnumerator spawnEnemyWithWait(float waitTime, int enemyIndex, Wave wave)
    {  
        for (int i = 0; i < wave.enemies[enemyIndex].amount; i++)
        {
            GameObject newEnemy = GameObject.Instantiate(baseEnemyPrefab, transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyController>().enemy = wave.enemies[enemyIndex].enemy;
            if(i == wave.enemies[enemyIndex].amount -1f)
            {
                print("final Enemy");
                gameController.currentWave += 1;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}
