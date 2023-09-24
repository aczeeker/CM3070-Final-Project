using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningEffect : MonoBehaviour
{
    [SerializeField]
    private int burnStack; // Current stack of the burning effect
    public int BurnStack => burnStack; // Property to access the burnStack value

    [SerializeField]
    private int maxBurnStack; // Maximum stack of the burning effect
    public int MaxBurnStack => maxBurnStack; // Property to access the maxBurnStack value

    [SerializeField]
    private float damagePerTick; // Damage inflicted per tick of the burning effect
    [SerializeField]
    private float[] durationsOfFires; // Array to store the remaining durations of active fires
    [SerializeField]
    private float timePerTick; // Time between each tick of the burning effect
    private float timePerTick_origin; // Original time between ticks

    private GameObject attachTo; // The GameObject this script is attached to
    private Enemy enemy; // Reference to the enemy script
    private bool initialied; // Flag to check if the object has been initialized

    /**
     * The user has to call initialize(float, float, float) right after instantiate(), 
     * otherwise the gameObject will be destroyed. 
     */
    private void Awake()
    {
        attachTo = transform.parent.gameObject;
        if (!attachTo.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Destroy the object if it's not attached to an enemy
        }
        enemy = attachTo.GetComponent<Enemy>(); // Get the Enemy component from the attached GameObject
        timePerTick_origin = timePerTick; // Store the original timePerTick
        durationsOfFires = new float[maxBurnStack]; // Initialize the durations array
    }

    public void initialize(float tickLength, float damageAmount, float totalDuration)
    {
        if (!initialied)
        {
            initialied = true;
            damagePerTick = damageAmount; // Set the damage per tick
            timePerTick = tickLength; // Set the time between ticks
            addBurning(totalDuration); // Add an initial burning effect
        }
    }

    /**
     * Add a new fire to the effect
     */
    public void addBurning(float dur, int n = 1)
    {
        for (int i = 0; i < n; i++)
        {
            for (int index = 0; index < durationsOfFires.Length; index++)
            {
                if (durationsOfFires[index] <= 0)
                {
                    durationsOfFires[index] = dur; // Set the duration of an available fire slot
                    break;
                }
                else if (durationsOfFires[index] <= dur)
                {
                    durationsOfFires[index] = dur; // Update the duration of an existing fire slot if needed
                }
            }
        }
    }

    public void DestroyBurn()
    {
        Destroy(gameObject); // Destroy the burning effect object
    }

    // Update is called once per frame
    void Update()
    {
        timePerTick -= Time.deltaTime; // Count down the time to the next tick

        if (timePerTick <= 0)
        {
            enemy.ReceiveDamage(damagePerTick * burnStack); // Deal damage to the enemy based on the burn stack
            timePerTick = timePerTick_origin; // Reset the time for the next tick
        }

        int currActiveFires = 0;
        for (int i = 0; i < durationsOfFires.Length; i++)
        {
            if (durationsOfFires[i] > 0)
            {
                durationsOfFires[i] -= Time.deltaTime; // Reduce the remaining duration of each active fire
                currActiveFires++;
            }
        }
        burnStack = currActiveFires; // Update the burn stack based on the number of active fires

        if (burnStack <= 0)
        {
            Debug.Log("Fire dying");
            Destroy(gameObject); // Destroy the object when there are no more active fires
        }
    }
}
