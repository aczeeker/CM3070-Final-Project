using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModularSpear : Spear
{
    // Reference to the image representing the spear.
    public GameObject image;

    // Flags indicating if the spear is in the shop or inventory.
    public bool inShop;
    public bool inInventory;

    // Flag to check if the spear has been initiated.
    private bool initiated;

    // References to the inventory systems.
    Inventory inv;
    Inventory shopInv;

    // Indices for spear components (tip, sash, handle, aura).
    int tip;
    int sash;
    int handle;
    int aura;

    // Aura type (0 = no aura, 1 = fire, 2 = cold).
    public int auraType;

    // Arrays of sprites for spear components (tip, sash, handle, aura).
    public Sprite[] tips;
    public Sprite[] sashes;
    public Sprite[] handles;
    public Sprite[] auras;

    // Arrays of values for spear component bonuses.
    public float[] tipVals;   // determines damage of weapon
    public float[] sashVals;   // determines damage reduction, or damage of weapon
    public float[] guardVals;    // determines speed of weapon

    // Cold effect values.
    [HideInInspector]
    public float slowDuration;
    [HideInInspector]
    public float freezeDuration;

    // Start is called before the first frame update
    protected override void Start()
    {
        if (initiated)
        {
            return;
        }

        // Calculate scaling factors based on the player's level.
        float toMult = 1 + (LevelInfo.level / 2f);
        for (int i = 0; i < tipVals.Length; i++)
        {
            tipVals[i] *= toMult;
            sashVals[i] *= toMult;
            guardVals[i] *= 1 + (LevelInfo.level / 8f);
        }

        // Initialize the spear.
        base.Start();
        tip = Random.Range(0, tips.Length);
        sash = Random.Range(0, sashes.Length);
        handle = Random.Range(0, handles.Length);

        // Set the sprite for each component.
        transform.Find("Tip").GetComponent<SpriteRenderer>().sprite = tips[tip];
        transform.Find("Sash").GetComponent<SpriteRenderer>().sprite = sashes[sash];
        transform.Find("Handle").GetComponent<SpriteRenderer>().sprite = handles[handle];

        if (!inInventory)
        {
            // Determine if the spear has an aura and apply it.
            if (Random.Range(0, 1.0f) <= auraChance) // messy and balanced for first floor only. TODO scale up
            {
                aura = Random.Range(0, auras.Length);
                auraType = aura + 1;
                transform.Find("Aura").GetComponent<SpriteRenderer>().sprite = auras[aura];
                additionalDamage += 5;
            }
            else
            {
                transform.Find("Aura").GetComponent<SpriteRenderer>().enabled = false;
                auraType = 0;
            }
        }

        // Calculate the total additional damage and update the description.
        additionalDamage += tipVals[tip] + sashVals[sash];
        attackSpeedMultiplier = guardVals[handle];
        description = "* +" + additionalDamage + " damage\n* x" + attackSpeedMultiplier + " attack speed";

        if (auraType == 1)
        {
            description += "\n* deals damage over time";
        }
        else if (auraType == 2)
        {
            description += "\n* may slow enemy";
            slowDuration = 2;
            freezeDuration = 1;
        }

        // Calculate the value of the spear.
        value = 10 * (tip + 1) + 5 * (handle + 1) + 2 * (sash + 1) + (int)additionalDamage;

        if (auraType != 0)
        {
            value += 100;
        }
        value = (int)System.Math.Round(value * toMult);

        // Get references to inventory systems.
        inv = GameObject.Find("InventoryCanvas").transform.Find("Inventory").gameObject.GetComponent<Inventory>();
        shopInv = GameObject.Find("ShopInventoryCanvas").transform.Find("ShopInventory").gameObject.GetComponent<Inventory>();
        initiated = true;

        // Set the scaling type.
        if (weaponScaling == 0)
        {
            scaling = "";
        }
        else if (weaponScaling == 1)
        {
            scaling = "Strength";
        }
        else if (weaponScaling == 2)
        {
            scaling = "Dexterity";
        }
        else if (weaponScaling == 3)
        {
            scaling = "Intelligence";
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (inShop)
        {
            return;
        }
        GameObject other = c.gameObject;
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            GameObject inventoryItem = Instantiate(image);
            inventoryItem.transform.Find("Tip").GetComponent<Image>().sprite = tips[tip];
            inventoryItem.transform.Find("Sash").GetComponent<Image>().sprite = sashes[sash];
            inventoryItem.transform.Find("Handle").GetComponent<Image>().sprite = handles[handle];

            if (auraType != 0)
            {
                inventoryItem.transform.Find("Aura").GetComponent<Image>().sprite = auras[aura];
            }
            else
            {
                inventoryItem.transform.Find("Aura").GetComponent<Image>().enabled = false;
            }

            ModularSpear invSpear = inventoryItem.GetComponent<ModularSpear>();
            invSpear.inInventory = true;
            invSpear.additionalDamage = additionalDamage;
            invSpear.attackSpeedMultiplier = attackSpeedMultiplier;
            invSpear.description = description;
            invSpear.itemName = "Spear";
            invSpear.value = value;
            invSpear.auraType = auraType;
            invSpear.slowDuration = slowDuration;
            invSpear.freezeDuration = freezeDuration;

            inv.AddItem(invSpear);
            Destroy(inventoryItem);
            Destroy(gameObject);
        }
    }

    public override void shopActivate(Transform slotTransform)
    {
        GameObject inventoryItem = Instantiate(image);
        inventoryItem.transform.Find("Tip").GetComponent<Image>().sprite = tips[tip];
        inventoryItem.transform.Find("Sash").GetComponent<Image>().sprite = sashes[sash];
        inventoryItem.transform.Find("Handle").GetComponent<Image>().sprite = handles[handle];
        inventoryItem.transform.Find("Aura").GetComponent<Image>().enabled = false;
        ModularSpear invSpear = inventoryItem.GetComponent<ModularSpear>();
        invSpear.additionalDamage = additionalDamage;
        invSpear.attackSpeedMultiplier = attackSpeedMultiplier;
        invSpear.description = description;
        invSpear.itemName = "Spear";
        invSpear.value = value;
        invSpear.auraType = auraType;
        invSpear.slowDuration = slowDuration;
        invSpear.freezeDuration = freezeDuration;

        Instantiate(invSpear, slotTransform);
        Destroy(inventoryItem);
        Destroy(gameObject);
    }

    public override void equip(GameObject g)
    {
        base.equip(g);
        g.GetComponent<Player>().ChangeAura(auraType);
        g.GetComponent<Player>().ChangeColdValues(slowDuration, freezeDuration);
    }

    public override void unequip(GameObject g)
    {
        base.unequip(g);
        g.GetComponent<Player>().ChangeAura(0);
        g.GetComponent<Player>().ChangeColdValues(0, 0);

    }
}
