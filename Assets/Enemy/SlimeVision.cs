using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeVision : MonoBehaviour
{
    [HideInInspector]
    public bool stalking;
    [HideInInspector]
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called when another Collider2D enters the vision area
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            if (!stalking)
            {
                // Start stalking the player if not already stalking
                stalking = true;
                target = collider.gameObject;
            }
        }
    }

    // Called when another Collider2D exits the vision area
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            // Stop stalking when the player exits the vision area
            stalking = false;
        }
    }
}
