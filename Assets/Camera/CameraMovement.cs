using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private bool isMoving; // A flag to check if the camera is currently moving

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false; // Initialize the flag to false when the script starts
    }

    // Public method to move the camera to a new position
    public void moveTo(Vector3 newPosition)
    {
        if (!isMoving) // Check if the camera is not already moving
        {
            StartCoroutine(MoveCamera(newPosition)); // Start the coroutine to move the camera
        }
    }

    // Coroutine to smoothly move the camera to a new position
    IEnumerator MoveCamera(Vector3 newPosition)
    {
        Vector3 start = transform.position; // Get the current position of the camera as the starting point
        float elapsedTime = 0; // Initialize a timer for interpolation
        isMoving = true; // Set the flag to indicate that the camera is moving

        // Continue moving the camera until it reaches the new position
        while (transform.position != newPosition)
        {
            // Interpolate between the starting and target positions over time
            Vector3 newPos = Vector3.Lerp(start, newPosition, elapsedTime / 1);
            transform.position = newPos; // Update the camera's position
            elapsedTime += Time.deltaTime; // Increment the timer
            yield return null; // Wait for the next frame
        }

        isMoving = false; // Reset the flag to indicate that the camera has stopped moving

        // Find the "Player" GameObject and access its "isCameraMoving" property through the "Player" component
        GameObject.Find("Player").GetComponent<Player>().isCameraMoving = false;
    }
}
