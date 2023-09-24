using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{
    // Additional damage granted by the sword.
    public float additionalDamage;

    // Attack speed multiplier for the sword.
    public float attackSpeedMultiplier;

    // Set the type of the item when it starts.
    protected virtual void Start()
    {
        type = "Weapon";
    }

    // Equip the sword on the player.
    public override void equip(GameObject g)
    {
        // Get the Player component from the GameObject.
        Player p = g.GetComponent<Player>();

        // Get the scaling stat based on the weapon's scaling type.
        int scaleStat = p.getStat(weaponScaling);

        // Increase player's damage and adjust attack speed when equipped with the sword.
        p.ChangeDamage(additionalDamage * (1f + scaleStat / 20f));
        p.RefactorSpeed(1 / attackSpeedMultiplier);
        p.ChangeCombatMode(0);
    }

    // Unequip the sword from the player.
    public override void unequip(GameObject g)
    {
        // Get the Player component from the GameObject.
        Player p = g.GetComponent<Player>();

        // Get the scaling stat based on the weapon's scaling type.
        int scaleStat = p.getStat(weaponScaling);

        // Decrease player's damage and restore attack speed when unequipped.
        g.GetComponent<Player>().ChangeDamage(-(additionalDamage * (1f + scaleStat / 20f)));
        g.GetComponent<Player>().RefactorSpeed(attackSpeedMultiplier);
        g.GetComponent<Player>().ChangeCombatMode(0);
    }

    // Activate the sword in a shop (not implemented in this version).
    public override void shopActivate(Transform slotTransform)
    {
        return;
    }
}
