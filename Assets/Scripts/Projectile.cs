using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goblin"))
        {
            other.GetComponent<Goblin>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}