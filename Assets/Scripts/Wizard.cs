using UnityEngine;

public class Wizard : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 1.5f;

    public int maxHealth = 5;
    public int health = 5;

    float fireTimer = 0f;
    int enemiesInRange = 0;

    void Update()
    {
        if (enemiesInRange > 0)
        {
            fireTimer += Time.deltaTime;

            if (fireTimer >= fireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, transform.position + Vector3.right * 0.5f, Quaternion.identity);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void EnemyEntered()
    {
        enemiesInRange++;

        // Shoot immediately when first enemy enters
        if (enemiesInRange == 1)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    public void EnemyExited()
    {
        enemiesInRange--;
    }
}