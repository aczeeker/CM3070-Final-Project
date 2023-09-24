using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    // Reference to the currently detected target (aggro)
    public GameObject aggro { get; private set; }

    private CircleCollider2D cr_collider;

    [SerializeField]
    private Enemy whose; // Reference to the owning enemy (optional)

    private void Start()
    {
        // Get the CircleCollider2D component attached to this GameObject
        cr_collider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the detected object is named "Player" (assuming the player's GameObject has this name)
        if (collider.gameObject.name == "Player")
        {
            // Set the detected target as aggro
            aggro = collider.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // Check if the previously detected target exits the trigger area
        if (collider.gameObject == aggro)
        {
            // Reset aggro (no longer detecting the target)
            aggro = null;
        }
    }

    // Coroutine to temporarily increase the detection radius
    public IEnumerator alertSearch()
    {
        // Store the original radius of the CircleCollider2D
        float originRadius = cr_collider.radius;

        // Increase the radius (10x times, for example)
        cr_collider.radius *= 10;

        // Wait for a short duration (0.3 seconds in this case)
        yield return new WaitForSeconds(0.3f);

        // Restore the original radius
        cr_collider.radius = originRadius;
    }
}
