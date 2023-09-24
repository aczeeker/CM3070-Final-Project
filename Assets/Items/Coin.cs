using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : WorldPotion
{
    // This method is called when the coin collides with a trigger collider.
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        // Call the base class's OnTriggerEnter2D method, if needed.
        base.OnTriggerEnter2D(collider);

        GameObject other = collider.gameObject;

        // Check if the collision is with the player.
        if (other.CompareTag("Player"))
        {
            // Get the Player component from the collided GameObject.
            Player player = other.GetComponent<Player>();

            // Add a coin to the player's inventory or update their coin count.
            player.addCoin();
        }
    }
}
