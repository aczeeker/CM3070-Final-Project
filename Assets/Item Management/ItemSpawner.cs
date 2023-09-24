using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    static Inventory inv;

    [SerializeField]
    GameObject newItem;

    public Sprite[] blades;
    public Sprite[] guards;
    public Sprite[] hilts;

    public float[] bladeVals;   // Determines damage of the weapon
    public float[] guardVals;   // Determines damage reduction or damage of the weapon
    public float[] hiltVals;    // Determines the speed of the weapon

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the inventory reference (you can assign it in the Inspector)
        //inv = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            // Get the Sword component from the newly generated object
            // GenerateSword();
        }
    }

    public void GenerateSword(Transform position)
    {
        // Instantiate a new sword object
        GameObject newObj = Instantiate(newItem, position);
        int blade = Random.Range(0, blades.Length);
        int guard = Random.Range(0, guards.Length);
        int hilt = Random.Range(0, hilts.Length);

        // Set the sprites for the blade, guard, and hilt
        newObj.transform.Find("Blade").GetComponent<SpriteRenderer>().sprite = blades[blade];
        newObj.transform.Find("Guard").GetComponent<SpriteRenderer>().sprite = guards[guard];
        newObj.transform.Find("Hilt").GetComponent<SpriteRenderer>().sprite = hilts[hilt];

        // Get the Sword component from the newly generated object
        Sword s = newObj.GetComponent<Sword>();
        s.type = "Weapon";
        s.additionalDamage = bladeVals[blade];
        s.attackSpeedMultiplier = hiltVals[hilt];
        s.description = "* +" + s.additionalDamage + " damage\n* x" + s.attackSpeedMultiplier + " attack speed";

        // Add the generated sword to the inventory
        AddToInventory(s);
    }

    public static void AddToInventory(Item i)
    {
        // Add the item to the inventory
        inv.AddItem(i);
    }
}
