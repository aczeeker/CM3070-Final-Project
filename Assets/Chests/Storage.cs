using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private bool storageOpen; // Flag to track whether the storage is open or closed
    private Canvas storageInventory; // Reference to the canvas for the storage inventory

    private Canvas inventoryCanvas; // Reference to the main inventory canvas

    // Start is called before the first frame update
    void Start()
    {
        // Find and assign references to the inventory canvases
        inventoryCanvas = GameObject.Find("InventoryCanvas").GetComponent<Canvas>();
        storageInventory = GameObject.Find("StorageInventoryCanvas").GetComponent<Canvas>();

        // Initialize the storage inventory as disabled and the storage as closed
        storageInventory.enabled = false;
        storageOpen = false;
    }

    #region helper_functions
    // Coroutine to handle opening and closing the storage
    IEnumerator OpenStorage()
    {
        yield return null;

        if (storageOpen) // If the storage is open
        {
            // Close the storage and resume the game
            storageInventory.enabled = false;
            storageOpen = false;
            inventoryCanvas.enabled = false;
            GameObject.Find("Game Manager").GetComponent<GameManager>().storageOpen = false;
            GameObject.Find("Game Manager").GetComponent<GameManager>().paused = false;
            Time.timeScale = 1; // Set the game's time scale to normal
            Enemy.setGamePause(false); // Resume enemy behavior
        }
        else // If the storage is closed
        {
            // Open the storage and pause the game
            storageInventory.enabled = true;
            storageOpen = true;
            inventoryCanvas.enabled = true;
            GameObject.Find("Game Manager").GetComponent<GameManager>().storageOpen = true;
            GameObject.Find("Game Manager").GetComponent<GameManager>().paused = true;
            Time.timeScale = 0; // Set the game's time scale to zero (pausing the game)
            Enemy.setGamePause(true); // Pause enemy behavior
        }
    }

    // Function to be called when interacting with the storage (e.g., opening/closing)
    public void Interact()
    {
        StartCoroutine(OpenStorage()); // Start the OpenStorage coroutine when interacting with the storage
    }
    #endregion
}
