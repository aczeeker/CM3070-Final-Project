using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SellSlot : MonoBehaviour, IDropHandler
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
        Transform t = DragHandler.itemBeingDragged.transform;

        // Check if the item being dragged originates from a "Shop" slot
        if (t.GetComponent<DragHandler>().getStartParent().GetComponent<ItemSlot>().type.Equals("Shop"))
        {
            // Item came from the shop, so return (do not sell it back)
            return;
        }

        // Get a reference to the dropped item
        GameObject droppedItem = DragHandler.itemBeingDragged;

        // Get the value (price) of the item
        int value = droppedItem.GetComponent<Item>().value;

        // Add the item's value to the player's coins
        inv.player.GetComponent<Player>().addCoins(value);

        // Destroy the dropped item, effectively selling it
        Destroy(droppedItem);
    }
}
