using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    public Recipe craftingRecipe;
    public Inventory inventory;

    public bool CanCraft()
    {
        foreach (ItemAmount itemAmount in craftingRecipe.requiredItems)
        {
            if (ItemCount(itemAmount.item) < itemAmount.amount)
            {
                return false;
            }
        }
        return true;
    }

    public void CraftFunction()
    {
        if (CanCraft())
        {
            foreach (ItemAmount itemAmount in craftingRecipe.requiredItems)
            {
                for (int i = 0; i < itemAmount.amount; i++)
                {
                    inventory.items.Remove(itemAmount.item);
                }
            }

            foreach (ItemAmount itemAmount in craftingRecipe.result)
            {
                for (int i = 0; i < itemAmount.amount; i++)
                {
                    inventory.items.Add(itemAmount.item);
                }
            }
            inventory.RefreshUI();
        }
    }

    public int ItemCount(Item item)
    {
        int num = 0;
        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i] == item)
            {
                num++;
            }
        }
        return num;
    }
}
