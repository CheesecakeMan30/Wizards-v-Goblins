using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 1;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goblin"))
        {
            Goblin goblin = other.GetComponent<Goblin>();

            if (goblin != null)
            {
                goblin.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}