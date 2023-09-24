using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{
    // Additional health provided by the armor when equipped.
    public float additionalHealth;

    // Equip the armor on the player GameObject.
    public override void equip(GameObject player)
    {
        if (player.GetComponent<Player>().health == player.GetComponent<Player>().maxhealth)
        {
            // If player's health is already at max, increase health directly.
            player.GetComponent<Player>().health += additionalHealth;
        }
        // Increase the player's maximum health when equipping the armor.
        player.GetComponent<Player>().maxhealth += additionalHealth;
    }

    // Unequip the armor from the player GameObject.
    public override void unequip(GameObject player)
    {
        // Decrease the player's maximum health when unequipping the armor.
        player.GetComponent<Player>().maxhealth -= additionalHealth;

        // If the player's health exceeds the new maximum, reduce their health.
        if (!(player.GetComponent<Player>().health - additionalHealth <= 0))
        {
            player.GetComponent<Player>().health -= additionalHealth;
        }
    }

    // Activation behavior when the item is in a shop's inventory.
    public override void shopActivate(Transform slotTransform)
    {
        // Implement any shop-specific activation behavior if needed.
    }
}
