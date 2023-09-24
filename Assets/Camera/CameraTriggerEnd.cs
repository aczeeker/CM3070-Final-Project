using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerEnd : MonoBehaviour
{
    public bool isStartingRoom; // A public flag to indicate if this is a starting room
    public bool isSafeRoom; // A public flag to indicate if this room is considered safe

    // Called when a 2D collider triggers with another collider
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player")) // Check if the colliding object has the "Player" tag
        {
            Player player = coll.gameObject.GetComponent<Player>(); // Get the Player component from the colliding object

            player.isChangingRoom = false; // Set a flag in the Player component to indicate that the room change is not happening

            if (!isStartingRoom && !isSafeRoom) // Check if this is not a starting room and not a safe room
            {
                // Find and access the "Enemy Spawner" component in the parent of this object
                EnemySpawner enemySpawner = transform.parent.Find("Enemy Spawner").GetComponent<EnemySpawner>();
                Transform enemySpawnerTransform = enemySpawner.transform;

                Debug.Log(transform.parent.name); // Output the name of the parent object to the console

                // Activate the "Door Blocks" GameObject within the parent
                transform.parent.Find("Door Blocks").gameObject.SetActive(true);

                // Call a method to spawn enemies using the EnemySpawner component
                enemySpawner.spawnEnemies();

                isSafeRoom = true; // Set the flag to indicate that this room is now considered safe
            }
        }
    }
}
