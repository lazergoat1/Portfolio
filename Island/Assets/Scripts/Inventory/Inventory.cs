using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;
    [Space]
    public Transform itemsParent;
    public ItemSlot[] itemSlots;
    [Space]
    public Transform equipmentSlotParent;
    public EquipmentSlot[] equipmentSlots;


    //All of this code is just for the editor 
    private void OnValidate()
    {
        if (itemsParent != null)
        {
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
        }
        if (equipmentSlotParent != null)
        {
            equipmentSlots = equipmentSlotParent.GetComponentsInChildren<EquipmentSlot>();
        }
        RefreshUI();
    }

    //This sets the item for the slots script
    public void RefreshUI()
    {
        int i = 0;
        for (; i < items.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].item = items[i];
        }
        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].item = null;
        }
    }

    public bool AddItem(Item item)
    {
        if (ItemCount() < itemSlots.Length)
        {
            items.Add(item);
            RefreshUI();
            return true;
        }
        return false;
    }

    public int ItemCount()
    {
        int num = 0;
        for (int i = 0; i < items.Count; i++)
        {
            num++;
        }
        return num;
    }
}
