using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : ItemSlot
{
    public EquipmentType equipmentType;
    //Create the tool type here it does not have to interfere with the players equipment type

    private void OnValidate()
    {
        gameObject.name = equipmentType.ToString() + "Slot";
    }
}
