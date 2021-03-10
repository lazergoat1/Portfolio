using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Amount
{
    public Enemy enemy;
    public float amount;
}

[CreateAssetMenu]
public class Wave : ScriptableObject
{
    public List<Amount> enemies;
    public float timeBetweenSpawn;
}
