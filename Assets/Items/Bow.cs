using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Item
{
    // Additional damage provided by the bow when equipped.
    public float additionalDamage;

    // Attack speed multiplier applied when the bow is equipped.
    public float attackSpeedMultiplier;

    // Range of the bow's attacks.
    public float range;

    // Projectile to be used when attacking with the bow.
    public GameObject projectile;

    // Equip the bow on the player GameObject.
    public override void equip(GameObject player)
    {
        Player p = player.GetComponent<Player>();
        int scaleStat = p.getStat(weaponScaling);
        p.ChangeDamage(additionalDamage * (1f + scaleStat / 20f));
        p.RefactorSpeed(1 / attackSpeedMultiplier);
        p.ChangeCombatMode(1);
        p.ChangeProjectile(projectile);
    }

    // Unequip the bow from the player GameObject.
    public override void unequip(GameObject player)
    {
        Player p = player.GetComponent<Player>();
        int scaleStat = p.getStat(weaponScaling);
        p.ChangeDamage(-(additionalDamage * (1f + scaleStat / 20f)));
        p.RefactorSpeed(attackSpeedMultiplier);
        p.ChangeCombatMode(0);
        p.ChangeProjectile(null);
    }

    // Activation behavior when the item is in a shop's inventory.
    public override void shopActivate(Transform slotTransform)
    {
        return;
    }
}
