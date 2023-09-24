using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField]
    private Skeleton skeleton; // Reference to the Skeleton script controlling the enemy

    // Public method to check if damage has been dealt
    public bool dealtDamage()
    {
        return m_dealtDamage;
    }

    // Private variable to track if damage has been dealt
    public bool m_dealtDamage { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        m_dealtDamage = false; // Initialize the damage dealt flag as false
    }

    // Update is called once per frame
    void Update()
    {
        // Define rotations for each orientation
        Quaternion up = Quaternion.Euler(0, 0, 90),
                 left = Quaternion.Euler(0, 0, 180),
                 down = Quaternion.Euler(0, 0, -90),
                 right = Quaternion.Euler(0, 0, 0);

        // Set the rotation of the hitbox based on the Skeleton's orientation
        if (skeleton.Orientation == Vector2.up)
        {
            transform.rotation = up;
        }
        else if (skeleton.Orientation == Vector2.left)
        {
            transform.rotation = left;
        }
        else if (skeleton.Orientation == Vector2.down)
        {
            transform.rotation = down;
        }
        else if (skeleton.Orientation == Vector2.right)
        {
            transform.rotation = right;
        }
    }

    // Called when the GameObject is enabled
    void OnEnable()
    {
        m_dealtDamage = false; // Reset the damage dealt flag when the hitbox is enabled
    }

    // Called when the hitbox triggers with another Collider2D
    private void OnTriggerEnter2D(Collider2D c)
    {
        GameObject other = c.gameObject;
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.TakeDamage(skeleton.AttackDamage); // Deal damage to the player
            m_dealtDamage = true; // Set the damage dealt flag to true
        }
    }
}
