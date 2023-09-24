using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    // Type of the item (e.g., "Weapon," "Armor," etc.).
    public string type;

    // Name of the item.
    public string itemName;

    // Value of the item.
    public int value;

    // Minimum value of the item.
    public int minValue;

    // Maximum value of the item.
    public int maxValue;

    // Weapon scaling type (e.g., 0 for None, 1 for Strength, 2 for Dexterity, 3 for Intelligence).
    public int weaponScaling;

    // Scaling attribute associated with the item.
    public string scaling;

    // Description of the item (multi-line text field).
    [TextArea]
    public string description;

    // Abstract method to equip the item. Subclasses must implement this.
    public abstract void equip(GameObject g);

    // Abstract method to unequip the item. Subclasses must implement this.
    public abstract void unequip(GameObject g);

    // Abstract method to activate the item in a shop or store context. Subclasses must implement this.
    public abstract void shopActivate(Transform slotTransform);
}
