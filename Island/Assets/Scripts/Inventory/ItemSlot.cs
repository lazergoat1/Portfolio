using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public Image image;
    public Inventory inventory;

    private Item _item;
    public Item item
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;
            if (_item == null)
            {
                image.enabled = false;
            }
            else
            {
                image.sprite = item.icon;
                image.enabled = true;
            }
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (this.GetType() == typeof(ItemSlot))
            {
                if (item.GetType() == typeof(EquippableItem))
                {     
                    AddItem((EquippableItem)item);
                }
            }
            else
            {
                RemoveItem((EquippableItem)item);
            }
        }
    }

    public void AddItem(EquippableItem item)
    {
        for (int i = 0; i < inventory.equipmentSlots.Length; i++)
        {
            if (inventory.equipmentSlots[i].equipmentType == item.equipmentType && inventory.equipmentSlots[i].item == null)
            {
                inventory.equipmentSlots[i].item = item;
                inventory.items.Remove(item);
                this.item = null;
                inventory.RefreshUI();
                break;
            }
        }
    }

    public void RemoveItem(EquippableItem item)
    {
        for (int i = 0; i < inventory.itemSlots.Length; i++)
        {
            if (inventory.itemSlots[i].item == null)
            {
                inventory.AddItem(item);
                this.item = null;
                inventory.RefreshUI();
                break;
            }
        }   
    }

    private void OnValidate()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
    }
}
