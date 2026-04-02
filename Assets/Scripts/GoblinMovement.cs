using UnityEngine;

public class Goblin : MonoBehaviour
{
    public float speed = 1.5f;

    public int maxHealth = 5;
    public int health = 5;

    public int damage = 1;
    public float attackRate = 1f;

    float attackTimer = 0f;
    bool attacking = false;

    Wizard targetWizard;
    HealthBar healthBar;

    void Start()
    {
        // Find health bar
        healthBar = GetComponentInChildren<HealthBar>();

        if (healthBar != null)
        {
            healthBar.SetHealth(health, maxHealth);
        }
    }

    void Update()
    {
        if (attacking)
        {
            // If wizard died, resume walking
            if (targetWizard == null)
            {
                attacking = false;
                return;
            }

            attackTimer += Time.deltaTime;

            if (attackTimer >= attackRate)
            {
                targetWizard.TakeDamage(damage);
                attackTimer = 0f;
            }
        }
        else
        {
            // Move left
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wizard"))
        {
            attacking = true;
            targetWizard = other.GetComponent<Wizard>();
            attackTimer = 0f;
        }

        if (other.CompareTag("Castle"))
        {
            Castle.instance.TakeDamage(1);
            GameManager.instance.GoblinKilled();
            Destroy(gameObject);
        }

        if (other.CompareTag("Projectile"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (healthBar != null)
        {
            healthBar.SetHealth(health, maxHealth);
        }

        if (health <= 0)
        {
            GameManager.instance.GoblinKilled();
            Destroy(gameObject);
        }
    }
}