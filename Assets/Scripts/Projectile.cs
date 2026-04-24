using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;

    [Header("Spin")]
    public bool spin = false;
    public float spinSpeed = 720f;

    [Header("Knockback")]
    public bool knockback = false;
    public float knockbackForce = 3f;
    public float knockbackDuration = 0.3f;

    [Header("Explosion / Splash Damage")]
    public bool explosive = false;
    public float explosionRadius = 1.5f;
    public int splashDamage = 5;

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (spin)
        {
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Goblin"))
            return;

        GoblinMovement goblin = other.GetComponent<GoblinMovement>();

        if (goblin != null)
        {
            // Direct hit damage
            goblin.TakeDamage(damage);

            // Direct hit knockback
            if (knockback)
            {
                goblin.ApplyKnockback(knockbackForce, knockbackDuration);
            }
        }

        // Splash explosion
        if (explosive)
        {
            Explode();
        }

        Destroy(gameObject);
    }

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Goblin"))
                continue;

            GoblinMovement goblin = hit.GetComponent<GoblinMovement>();

            if (goblin != null)
            {
                goblin.TakeDamage(splashDamage);

                // Optional: splash knockback too
                if (knockback)
                {
                    goblin.ApplyKnockback(knockbackForce, knockbackDuration);
                }
            }
        }
    }
}