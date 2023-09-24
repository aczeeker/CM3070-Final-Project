using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    [SerializeField]
    private float lifespan; // The duration of the spawn effect

    private float countdown; // Countdown timer for the lifespan of the effect

    // Start is called before the first frame update
    void Start()
    {
        countdown = lifespan; // Initialize the countdown timer with the specified lifespan
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime; // Decrease the countdown timer by the time elapsed in this frame

        if (countdown < 0)
        {
            // If the countdown timer reaches zero or below, destroy the gameObject
            Destroy(gameObject);
        }
    }
}
