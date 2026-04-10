using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;

    public bool spin = false;
    public float spinSpeed = 720f;

    public bool knockback = false;
    public float knockbackForce = 200f;

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
            // Damage
            var goblin = other.GetComponent<GoblinMovement>();
        if (goblin != null)
        {
        goblin.TakeDamage(damage);
        }

            // Knockback (wind wizard)
            if (knockback)
            {
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(Vector2.right * knockbackForce);
                }
                else
                {
                    // fallback if no rigidbody
                    other.transform.position += Vector3.right * 0.5f;
                }
            }

            Destroy(gameObject);
        }
    }
}