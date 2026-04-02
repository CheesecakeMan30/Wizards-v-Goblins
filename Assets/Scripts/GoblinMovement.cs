using UnityEngine;

public class Goblin : MonoBehaviour
{
    public float speed = 1.5f;
    public int health = 5;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

   public void TakeDamage(int dmg)
{
    health -= dmg;

    if (health <= 0)
    {
        GameManager.instance.GoblinKilled();
        Destroy(gameObject);
    }
}

   void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Castle"))
    {
        Castle.instance.TakeDamage(1);
        Destroy(gameObject);
        GameManager.instance.GoblinKilled();
    }
}
}