using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    private float m_speed;

    [SerializeField]
    private float lifeSpan;

    [SerializeField]
    private float wallHitTime;

    private Collider2D cr_collider;
    private SpriteRenderer cr_sr;
    private Rigidbody2D cr_rb;

    private bool fadingOut;

    public float AttackDamage { private get; set; }

    public float Speed => m_speed;

    private void Start()
    {
        // Get references to required components when the projectile is initialized
        cr_collider = GetComponent<Collider2D>();
        cr_rb = GetComponent<Rigidbody2D>();
        cr_sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // Countdown the projectile's lifespan
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
        {
            // If the lifespan is over, start fading out the projectile
            StartCoroutine(fadeOut());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            // If the projectile hits the player, deal damage to the player and destroy the projectile
            Player player = collision.collider.gameObject.GetComponent<Player>();
            player.TakeDamage(AttackDamage);
            Destroy(gameObject);
        }
        else if (collision.collider.gameObject.CompareTag("Walls"))
        {
            // If the projectile hits a wall, start fading out the projectile
            StartCoroutine(fadeOut());
        }
    }

    IEnumerator fadeOut()
    {
        if (!fadingOut)
        {
            fadingOut = true;
            cr_collider.enabled = false; // Disable the collider
            cr_rb.constraints = RigidbodyConstraints2D.FreezeAll; // Freeze rigidbody constraints
            Color toColor = new Color(cr_sr.color.r, cr_sr.color.g, cr_sr.color.b, 0); // Target color with alpha set to 0 (fully transparent)
            float wallHitTime_origin = wallHitTime;

            while (wallHitTime > 0)
            {
                // Gradually fade out the projectile by changing its color
                cr_sr.color = Color.Lerp(cr_sr.color, toColor, 1 - wallHitTime / wallHitTime_origin);
                wallHitTime -= Time.deltaTime;
                yield return null;
            }

            // Destroy the projectile after it has faded out
            Destroy(gameObject);
        }
    }
}
