using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpriteManager : MonoBehaviour
{
    #region Cached Reference
    private BurningEffect burningeffect; // Reference to the BurningEffect component
    private GameObject fireSprite; // Reference to the fire sprite GameObject
    #endregion

    #region Private Variables
    private int numFires; // Number of fire sprite instances
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        burningeffect = GetComponent<BurningEffect>(); // Get the BurningEffect component attached to the same GameObject
        fireSprite = transform.GetChild(0).gameObject; // Get the first child GameObject, assumed to be the fire sprite
        numFires = 1; // Initialize the number of fire sprite instances to 1
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure the number of fire sprite instances matches the burn stack
        while (numFires != burningeffect.BurnStack)
        {
            if (numFires < burningeffect.BurnStack)
            {
                addFireSprite(); // Add a fire sprite instance if there are fewer than the burn stack
                numFires++;
            }
            else
            {
                minusFireSprite(); // Remove a fire sprite instance if there are more than the burn stack
                numFires--;
            }
        }
    }

    private void minusFireSprite()
    {
        int childCount = transform.childCount;
        if (childCount > 1)
        {
            GameObject toBeRemoved = transform.GetChild(childCount - 1).gameObject;
            Destroy(toBeRemoved); // Remove the last fire sprite instance
        }
    }

    private void addFireSprite()
    {
        GameObject newFire = Instantiate(fireSprite, gameObject.transform); // Instantiate a new fire sprite instance
        float hShift = Random.Range(-0.5f, 0.5f);
        float vShift = Random.Range(-0.5f, 1);
        Vector2 shift = new Vector2(hShift, vShift);
        newFire.transform.position += (Vector3)shift; // Randomly shift the position of the new fire sprite
    }
}
