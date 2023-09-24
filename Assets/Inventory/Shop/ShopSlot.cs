using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopSlot : MonoBehaviour, IDropHandler
{
    private GameObject inventory;

    void Start()
    {
        // Subscribe to the sceneLoaded event to handle inventory initialization
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Find and get a reference to the Inventory GameObject
        inventory = GameObject.Find("InventoryCanvas").transform.Find("Inventory").gameObject;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reinitialize the reference to the Inventory GameObject when a new scene is loaded
        inventory = GameObject.Find("InventoryCanvas").transform.Find("Inventory").gameObject;
    }

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                // Return the first child of the slot if it exists (represents the item)
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Do nothing when an item is dropped onto this slot (it's a shop slot)
    }

    public void OnMouseEnter()
    {
        if (!item)
        {

        }
        else
        {
            // Display the item's description and name when the mouse enters
            inventory.GetComponent<Inventory>().descriptionText.text = item.GetComponent<Item>().description;
            inventory.GetComponent<Inventory>().itemName.text = item.GetComponent<Item>().itemName;
        }
    }

    public void OnMouseExit()
    {
        // Clear the displayed description and item name when the mouse exits
        inventory.GetComponent<Inventory>().descriptionText.text = "";
        inventory.GetComponent<Inventory>().itemName.text = "";
    }
}
