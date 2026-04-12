using UnityEngine;

public class GoblinMovement : MonoBehaviour
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

    // LANE LOCK
    public float laneY;

    // KNOCKBACK SYSTEM
    private float knockbackTimer = 0f;
    private float knockbackForce = 0f;

    // prevents double counting
    private bool isDead = false;

    void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();

        if (healthBar != null)
        {
            healthBar.SetHealth(health, maxHealth);
        }
    }

    void Update()
    {
        // HANDLE KNOCKBACK FIRST
        if (knockbackTimer > 0)
        {
            transform.Translate(Vector2.right * knockbackForce * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
        }
        else if (attacking)
        {
            if (targetWizard == null)
            {
                attacking = false;
            }
            else
            {
                attackTimer += Time.deltaTime;

                if (attackTimer >= attackRate)
                {
                    targetWizard.TakeDamage(damage);
                    attackTimer = 0f;
                }
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        // FORCE BACK TO LANE EVERY FRAME
        Vector3 pos = transform.position;
        pos.y = laneY;
        transform.position = pos;
    }

    // APPLY KNOCKBACK
    public void ApplyKnockback(float force, float duration)
    {
        knockbackForce = force;
        knockbackTimer = duration;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return; 

        if (other.CompareTag("Wizard"))
        {
            attacking = true;
            targetWizard = other.GetComponent<Wizard>();
            attackTimer = 0f;
        }

        if (other.CompareTag("Castle"))
        {
            Castle.instance.TakeDamage(1);
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
        if (isDead) return; // prevents multiple hits in same frame

        health -= dmg;

        if (healthBar != null)
        {
            healthBar.SetHealth(health, maxHealth);
        }

        if (health <= 0)
        {
            isDead = true;

            // disable collider immediately to stop extra hits
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;

            GameManager.instance.GoblinKilled();
            Destroy(gameObject);
        }
    }
}