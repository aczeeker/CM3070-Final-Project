using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Item
{
    // Additional damage granted by the spear.
    public float additionalDamage;

    // Attack speed multiplier for the spear.
    public float attackSpeedMultiplier;

    // Aura chance for the spear.
    public float auraChance;

    // Set the type of the item when it starts.
    protected virtual void Start()
    {
        type = "Weapon";
    }

    // Equip the spear on the player.
    public override void equip(GameObject g)
    {
        Debug.Log("equipped");

        // Get the Player component from the GameObject.
        Player p = g.GetComponent<Player>();

        // Get the scaling stat based on the weapon's scaling type.
        int scaleStat = p.getStat(weaponScaling);

        // Increase player's damage and adjust attack speed when equipped with the spear.
        p.ChangeDamage(additionalDamage * (1f + scaleStat / 20f));
        p.RefactorSpeed(1 / attackSpeedMultiplier);
        p.ChangeCombatMode(2);
    }

    // Unequip the spear from the player.
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

    // Activate the spear in a shop (not implemented in this version).
    public override void shopActivate(Transform slotTransform)
    {
        return;
    }
}
