using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Head,
    Chest,
    Hand,
}

[CreateAssetMenu]
public class EquippableItem : Item
{
    public float dmg;
    public float durability;
    [Space]
    public EquipmentType equipmentType;
}
