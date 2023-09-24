using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mimic : Enemy
{
    public override bool IsAttacking => disclosed;

    [SerializeField]
    private float alertThreshold;

    [SerializeField]
    private float attackFreeze;

    [SerializeField]
    private EnemyProjectile projectile;

    private float alertMeter;

    private bool disclosed;

    private Rigidbody2D rb;

    private Animator cr_anim;

    private GameObject player;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        gameObject.tag = "Untagged"; // Set the initial tag to "Untagged"
        alertMeter = 0;
        disclosed = false;
        rb = GetComponent<Rigidbody2D>();
        cr_anim = GetComponentInChildren<Animator>();
        cr_anim.SetBool("disclosed", false); // Set the "disclosed" parameter in the animator to false
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (alertMeter > alertThreshold)
        {
            if (!disclosed)
            {
                gameObject.tag = "Enemy"; // Set the tag to "Enemy" when alert threshold is reached
                StartCoroutine(Attack(player));
                disclosed = true;
                cr_anim.SetBool("disclosed", true); // Set the "disclosed" parameter in the animator to true
            }
        }
    }

    public override void ReceiveDamage(float damage)
    {
        base.ReceiveDamage(damage);
        Color damaged = new Color(1, 0.4f, 0.4f);
        StartCoroutine(base.flashColor(damaged, 0.5f)); // Flash the enemy's color when taking damage
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject; // Track the player when they enter the trigger zone
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            alertMeter += Time.deltaTime; // Increase alert meter when the player stays in the trigger zone
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            Rigidbody2D player_rb = other.GetComponent<Rigidbody2D>();
            Debug.Log("Player rb velocity is " + player_rb.velocity); // Log the player's rigidbody velocity on collision
        }
    }

    private IEnumerator Attack(GameObject other)
    {
        Vector3 ammoDir = other.transform.position - transform.position;
        ammoDir = ammoDir.normalized;
        var angle = Mathf.Atan2(ammoDir.y, ammoDir.x) * Mathf.Rad2Deg;
        EnemyProjectile ammo = Instantiate(projectile, transform.position, Quaternion.AngleAxis(angle - 90, Vector3.forward));
        ammo.AttackDamage = AttackDamage;
        ammo.GetComponent<Rigidbody2D>().velocity = (other.transform.position - transform.position).normalized * ammo.Speed;
        yield return new WaitForSeconds(attackFreeze);
        StartCoroutine(Attack(other)); // Continuously initiate attacks with a freeze time
    }
}
