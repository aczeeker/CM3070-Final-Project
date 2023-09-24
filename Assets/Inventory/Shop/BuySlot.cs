using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuySlot : MonoBehaviour, IDropHandler
{
    // Reference to the inventory system
    private Inventory inv;

    void Start()
    {
        // Find and get a reference to the Inventory script in the scene
        inv = GameObject.Find("InventoryCanvas").transform.Find("Inventory").gameObject.GetComponent<Inventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Get the GameObject that is being dragged (the item being dropped)
        GameObject droppedItem = DragHandler.itemBeingDragged;

        // Check if the player has enough coins to buy the item
        if (inv.player.GetComponent<Player>().getCoins() < droppedItem.GetComponent<Item>().value)
        {
            // Player doesn't have enough coins, so return (do not buy)
            return;
        }
        else
        {
            // Reduce the player's coins by the item's value
            inv.player.GetComponent<Player>().reduceCoins(droppedItem.GetComponent<Item>().value);

            // Get the next empty slot in the inventory
            Transform slotTransform = inv.getNextEmpty();

            if (slotTransform)
            {
                // Set the parent of the dropped item to the empty slot, effectively adding it to the inventory
                droppedItem.transform.SetParent(slotTransform);
            }
        }
    }
}
