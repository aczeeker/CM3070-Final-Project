using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    #region gameObject_variables
    [SerializeField]
    [Tooltip("Enemy spawn")]
    private GameObject mimic;

    [SerializeField]
    [Tooltip("Chance to spawn mimic")]
    private float mimicChance;

    [SerializeField]
    [Tooltip("Item")]
    private GameObject[] drop;

    [SerializeField]
    [Tooltip("Item chances")]
    private float[] dropRates;

    [SerializeField]
    [Tooltip("Coin chance")]
    private float coinChance;

    public GameObject coin; // Prefab for a single coin
    public int coinMin; // Minimum number of coins to spawn
    public int coinMax; // Maximum number of coins to spawn
    #endregion

    #region helper_functions
    // Coroutine to delete the chest and handle spawning of items or mimic
    IEnumerator DeleteChest()
    {
        yield return new WaitForSeconds(0.3f);

        if (Random.Range(0f, 1f) > mimicChance) // Determine if a mimic should spawn based on mimicChance
        {
            bool dropped = false;
            while (!dropped)
            {
                for (int itemToDrop = 0; itemToDrop < drop.Length; itemToDrop++)
                {
                    if (Random.value < dropRates[itemToDrop]) // Determine if an item should drop based on dropRates
                    {
                        dropped = true;
                        // Instantiate and spawn the selected item near the chest
                        Instantiate(drop[itemToDrop], transform.position + (Vector3)Random.insideUnitCircle, transform.rotation);
                    }
                }
                if (Random.value < coinChance) // Determine if coins should spawn based on coinChance
                {
                    dropped = true;
                    int coinsToSpawn = Random.Range(coinMin, coinMax); // Determine the number of coins to spawn
                    for (int i = 0; i < coinsToSpawn; i++)
                    {
                        // Instantiate and spawn a coin near the chest
                        Instantiate(coin, gameObject.transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
                    }
                }
            }
        }
        else
        {
            // If mimicChance is met, instantiate and spawn a mimic enemy at the chest's position
            Instantiate(mimic, transform.position, transform.rotation);
        }
        // Destroy the chest object after items and/or enemies are spawned
        Destroy(gameObject);
    }

    // Function to be called when the chest is interacted with (e.g., opened)
    public void Interact()
    {
        StartCoroutine(DeleteChest()); // Start the DeleteChest coroutine when the chest is interacted with
    }
    #endregion
}