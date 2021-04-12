using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class LoadLevelMenu : MonoBehaviour
{
    public PlayerStats playerStats;
    public WorldValues worldValues;

    public RectTransform viewportContent;
    public GameObject buttonPrefab;
    public List<GameObject> loadWorldButtons = new List<GameObject>();
    public GameObject loadButtonHider;
    public GameObject deleteButtonHider;

    public string loadSaveName;

    private void Awake()
    {
        GetAllSaveFiles();
        
    }
    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null)
        {
            loadSaveName = "";
            loadButtonHider.SetActive(true);
            loadButtonHider.GetComponentInParent<Button>().interactable = false;
            deleteButtonHider.SetActive(true);
            deleteButtonHider.GetComponentInParent<Button>().interactable = false;
        }
    }


    public void CreateLoadButton(string buttonName)
    {
        GameObject newButton = Instantiate(buttonPrefab, viewportContent.position, Quaternion.identity, viewportContent);
        newButton.name = buttonName;
        loadWorldButtons.Add(newButton);

        newButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonName;
        newButton.GetComponent<Button>().onClick.AddListener(delegate { SelectSave(buttonName); });
    }


    public void GetAllSaveFiles()
    {
        if (!Directory.Exists(Path.GetDirectoryName(Application.dataPath) + "/Saves"))
        {
            return;
        }

        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(Application.dataPath) + "/Saves/");
        FileInfo[] files = directoryInfo.GetFiles("*.save");

        // Sort by creation-time descending 
        Array.Sort(files, delegate (FileInfo f1, FileInfo f2)
        {
            return f2.LastAccessTime.CompareTo(f1.LastAccessTime);
        });

        if (files.Length > 0)
        {
            foreach (FileInfo file in files)
            {
                CreateLoadButton(file.Name.Substring(0, file.Name.Length - 5));
            }
        } 
    }

    public void SelectSave(string buttonName)
    {
        loadSaveName = buttonName;

        loadButtonHider.SetActive(false);
        loadButtonHider.GetComponentInParent<Button>().interactable = true;
        deleteButtonHider.SetActive(false);
        deleteButtonHider.GetComponentInParent<Button>().interactable = true;
    }

    public void DeleteSave()
    {
        string filePath = Path.GetDirectoryName(Application.dataPath) + "/Saves/" + loadSaveName + ".save";

        // check if file exists
        if (!File.Exists(filePath))
        {
            Debug.Log("no " + loadSaveName + " file exists");
        }
        else
        {
            Debug.Log(loadSaveName + " file exists, deleting...");

            File.Delete(filePath);

            //AssetDatabase.Refresh();
        }

        foreach (GameObject button in loadWorldButtons)
        {
            if (button.name == loadSaveName)
            {
                loadWorldButtons.Remove(button);
                Destroy(button);
                break;
            }
        }
    }

    public void LoadSave()
    {
        SaveData saveData = SerializationManager.Load(Path.GetDirectoryName(Application.dataPath) + "/Saves/" + loadSaveName + ".save") as SaveData;

        if (saveData != null && loadSaveName != "")
        {
            playerStats.currentHealth = saveData.health;
            playerStats.xp = saveData.xp;
            playerStats.level = saveData.level;

            GameManager.instance.worldValues.seed = saveData.seed;
            GameManager.instance.worldValues.worldName = loadSaveName;
            GameManager.instance.worldValues.newSave = false;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            playerStats.position = new Vector3(saveData.position[0], saveData.position[1], saveData.position[2]);
        }
    }
}
