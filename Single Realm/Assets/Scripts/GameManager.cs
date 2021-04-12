using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public WorldValues worldValues;

    public int autoSaveDelay;

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        StartCoroutine(AutoSave(autoSaveDelay));
    }

    public void Save()
    {
        SerializationManager.Save(worldValues.worldName, playerStats, worldValues);
    }

    public IEnumerator AutoSave(int delayBetweenSaves)
    {
        while(true)
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                Save();
            }
            yield return new WaitForSeconds(delayBetweenSaves);
        }
    }
}
