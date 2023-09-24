using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHealthPotion : WorldPotion
{
    #region Editor Variables
    [SerializeField]
    private float healingAmount; // The amount of health this potion will heal
    #endregion

    // This method is called when a 2D collider enters a trigger collider (e.g., when a player picks up the potion).
    protected override void OnTriggerEnter2D(Collider2D c)
    {
        base.OnTriggerEnter2D(c); // Call the OnTriggerEnter2D method of the parent class (WorldPotion).

        GameObject other = c.gameObject; // Get the GameObject that triggered the collider.

        // Check if the triggering GameObject has the "Player" tag.
        if (other.CompareTag("Player"))
        {
            // If the triggering GameObject is a player, get the Player component.
            Player player = other.GetComponent<Player>();

            // Call the ReceiveHealing method of the Player component, passing the healingAmount.
            player.ReceiveHealing(healingAmount);
        }
    }
}
