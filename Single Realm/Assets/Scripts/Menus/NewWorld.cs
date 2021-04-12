using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NewWorld : MonoBehaviour
{
    private WorldValues mapValues;
    public TMP_InputField seedInputField;
    public TMP_InputField worldNameInputField;

    private void Awake()
    {
        mapValues = GameObject.FindObjectOfType<WorldValues>();
    }
    public void CreateWorld()
    {
        if(seedInputField.text != "")
        {
            mapValues.seed = int.Parse(seedInputField.text);
        }
        else
        {
            mapValues.seed = new System.Random().Next(-1000000,1000000);
        }

        if (worldNameInputField.text != "")
        {
            CreateNewSave(worldNameInputField.text);
        }
        else
        {
            CreateNewSave("New World");
        }

        GameManager.instance.worldValues.newSave = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreateNewSave(string saveName)
    {
        SerializationManager.CreateNewSave(saveName, GameManager.instance.playerStats, GameManager.instance.worldValues);
    }
}
