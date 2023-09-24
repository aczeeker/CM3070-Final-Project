using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IHasChanged
{
    public Item[] inventory; // Array to store items in the inventory slots
    public Item[] equipment; // Array to store equipped items
    public GameObject player;
    [SerializeField] Transform equipmentSlots; // Reference to equipment slots in the UI
    [SerializeField] Transform inventorySlots; // Reference to inventory slots in the UI
    public Text descriptionText; // Text to display item description
    public Text itemName; // Text to display item name
    public Text itemValue; // Text to display item value
    public Text itemScaling; // Text to display item scaling

    void Start()
    {
        // Initialize the inventory and equipment arrays based on the number of slots
        inventory = new Item[inventorySlots.childCount];
        equipment = new Item[equipmentSlots.childCount];
        HasChanged();
    }

    // Called when a slot's item has changed
    public void HasChanged()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        int i = 0;

        // Check each equipment slot
        foreach (Transform slotTransform in equipmentSlots)
        {
            GameObject item = slotTransform.GetComponent<ItemSlot>().item;
            if (item)
            {
                Item eq = item.GetComponent<Item>();

                // Equip this item and call relevant methods in the Player script
                if (!equipment[i])
                {
                    equipment[i] = eq;
                    player.GetComponent<Player>().Equip(eq);
                }
                else if (equipment[i] != eq)
                {
                    player.GetComponent<Player>().Unequip(equipment[i]);
                    equipment[i] = eq;
                    player.GetComponent<Player>().Equip(eq);
                }

                builder.Append(item.name);
                builder.Append(" - ");
            }
            else
            {
                if (equipment[i])
                {
                    // Unequip the item and update the equipment array
                    player.GetComponent<Player>().Unequip(equipment[i]);
                }
                equipment[i] = null;
            }
            i++;
        }

        if (builder.Length > 0)
        {
            builder.Remove(builder.Length - 3, 3);
        }

        // Update the UI text to display equipped items
        // equipmentText.text = builder.ToString();

        i = 0;

        // Check each inventory slot
        foreach (Transform slotTransform in inventorySlots)
        {
            GameObject item = slotTransform.GetComponent<ItemSlot>().item;
            if (item)
            {
                Item inv = item.GetComponent<Item>();
                inventory[i] = inv;
            }
            else
            {
                inventory[i] = null;
            }
            i++;
        }
    }

    // Add an item to inventory slots
    public void AddItem(Item itemToAdd)
    {
        int i = 0;

        // Check each inventory slot
        foreach (Transform slotTransform in inventorySlots)
        {
            GameObject item = slotTransform.GetComponent<ItemSlot>().item;
            if (!item)
            {
                // Instantiate the item in the empty slot
                Instantiate(itemToAdd, slotTransform);
                return;
            }
            i++;
        }
    }

    public Transform getNextEmpty()
    {
        foreach (Transform slotTransform in inventorySlots)
        {
            GameObject item = slotTransform.GetComponent<ItemSlot>().item;
            if (!item)
            {
                return slotTransform;
            }
        }
        return null;
    }

    // Remove an item from inventory slots
    public void RemoveItem(Item itemToRemove)
    {
        int i = 0;

        // Check each inventory slot
        foreach (Transform slotTransform in inventorySlots)
        {
            if (inventory[i] == itemToRemove)
            {
                inventory[i] = null;
                Destroy(slotTransform.GetComponent<ItemSlot>().item);
            }
        }
    }
}

namespace UnityEngine.EventSystems
{
    public interface IHasChanged : IEventSystemHandler
    {
        void HasChanged();
    }
}
