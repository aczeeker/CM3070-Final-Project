using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class MoveableEnemy : Enemy
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("Movement speed of the enemy, in 1 unit length per second")]
    protected float m_speed = 5;
    #endregion

    #region Cached Components
    protected Rigidbody2D rb; // Reference to the Rigidbody2D component
    #endregion

    #region Public Read-only Properties
    public bool IsIdle => !IsMoving; // Check if the enemy is idle (not moving)
    abstract public bool IsMoving { get; } // Abstract property to determine if the enemy is moving
    #endregion

    #region Protected Methods
    protected virtual void move(Vector2 destination, float tolerance = 0.5f)
    {
        if (gamePaused)
        {
            return; // If the game is paused, do not perform any movement
        }
        Vector2 dir = destination - (Vector2)transform.position; // Calculate the direction to the destination
        if (dir.magnitude < tolerance)
        {
            dir = Vector2.zero; // If the distance to the destination is within tolerance, stop moving
        }
        else
        {
            dir = dir.normalized; // Normalize the direction vector
        }

        if (Vector2.Dot(rb.velocity, dir * m_speed) < (dir * m_speed).magnitude * (dir * m_speed).magnitude)
        {
            // Apply forces to the Rigidbody2D to move the enemy
            rb.AddForce(-rb.velocity * rb.mass);
            rb.AddForce(dir * m_speed * rb.mass * chilled); // Apply force in the direction of movement
        }
    }
    #endregion

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>(); // Get the reference to the Rigidbody2D component
    }
}
