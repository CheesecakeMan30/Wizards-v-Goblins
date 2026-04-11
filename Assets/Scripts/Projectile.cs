using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;

    public bool spin = false;
    public float spinSpeed = 720f;

    public bool knockback = false;
    public float knockbackForce = 3f;     // 🔥 changed (smaller, controlled)
    public float knockbackDuration = 0.3f; // 🔥 NEW

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
        if (other.CompareTag("Goblin"))
        {
            var goblin = other.GetComponent<GoblinMovement>();

            if (goblin != null)
            {
                // ✅ Damage
                goblin.TakeDamage(damage);

                // ✅ NEW knockback system
                if (knockback)
                {
                    goblin.ApplyKnockback(knockbackForce, knockbackDuration);
                }
            }

            Destroy(gameObject);
        }
    }
}