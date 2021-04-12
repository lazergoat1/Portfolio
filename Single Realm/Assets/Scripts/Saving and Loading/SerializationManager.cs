using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SerializationManager
{
    public static bool Save(string saveName, PlayerStats playerStats, WorldValues worldValues)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        if(!Directory.Exists(Path.GetDirectoryName(Application.dataPath) + "/Saves"))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Application.dataPath) + "/Saves");
        }

        string path = Path.GetDirectoryName(Application.dataPath) + "/Saves/" + saveName + ".save";

        FileStream file = new FileStream(path, FileMode.Create);

        SaveData saveData = new SaveData(playerStats, worldValues, saveName);

        formatter.Serialize(file, saveData);

        file.Close();

        return true;
    }

    public static bool CreateNewSave(string saveName, PlayerStats playerStats, WorldValues worldValues)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        if (!Directory.Exists(Path.GetDirectoryName(Application.dataPath) + "/Saves"))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Application.dataPath) + "/Saves");
        }

        string path = Path.GetDirectoryName(Application.dataPath) + "/Saves/" + saveName + ".save";

        if (File.Exists(path))
        {
            Debug.Log("There is already a world with this name");
            int numberOfDuplicates = 0;

            DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(Application.dataPath) + "/Saves/");
            FileInfo[] fileInfo = directoryInfo.GetFiles(saveName + "*.save");

            if (fileInfo.Length > 0)
            {
                for (int i = 0; i < fileInfo.Length; i++)
                {
                    numberOfDuplicates++;
                }
            }

            saveName = saveName + " " + numberOfDuplicates;

            path = Path.GetDirectoryName(Application.dataPath) + "/Saves/" + saveName + ".save";
        }

        FileStream file = new FileStream(path, FileMode.Create);

        SaveData saveData = new SaveData(playerStats, worldValues, saveName);
        worldValues.worldName = saveName;

        formatter.Serialize(file, saveData);

        file.Close();

        return true;
    }

    public static object Load(string path)
    {
        if(!File.Exists(path))
        {
            Debug.LogError("No file");
            return null;
        }

        BinaryFormatter formatter = GetBinaryFormatter();

        FileStream file = File.Open(path, FileMode.Open);

        try
        {
            object save = formatter.Deserialize(file);
            file.Close();
            return save;
        }
        catch
        {
            Debug.LogErrorFormat("Failed to load file at {0}" , path);
            file.Close();
            return null;
        }
    }

    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        return formatter;
    }
}
