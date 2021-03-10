using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemAmount
{
    public Item item;
    [Range(1, 999)]
    public float amount;
}

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public List<ItemAmount> requiredItems;
    public List<ItemAmount> result;
}
