using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float health;
    public float xp;
    public int level;
    public float[] position;

    public int seed;
    public string saveName;

    public SaveData(PlayerStats PlayerStats, WorldValues WorldValues, string SaveName)
    {
        health = PlayerStats.currentHealth;
        xp = PlayerStats.xp;
        level = PlayerStats.level;

        position = new float[3];
        position[0] = PlayerStats.position.x;
        position[1] = PlayerStats.position.y;
        position[2] = PlayerStats.position.z;

        seed = WorldValues.seed;
        saveName = SaveName;
    }

}
