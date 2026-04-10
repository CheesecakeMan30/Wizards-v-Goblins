using UnityEngine;
using System.Collections.Generic;

public class Wizard : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 1.5f;

    public int maxHealth = 5;
    public int health = 5;

    float fireTimer = 0f;

    List<GameObject> enemiesInRange = new List<GameObject>();

    public bool canShoot = true;

    void Update()
    {
        if (!canShoot) return;

        enemiesInRange.RemoveAll(e => e == null);

        if (enemiesInRange.Count > 0)
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

    public void EnemyEntered(GameObject enemy)
    {
        if (!canShoot) return;

        if (!enemiesInRange.Contains(enemy))
            enemiesInRange.Add(enemy);
    }

    public void EnemyExited(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    // 🔥 NEW: force detect enemies already inside
    public void DetectExistingEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 5f); // adjust radius if needed

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Goblin"))
            {
                EnemyEntered(hit.gameObject);
            }
        }
    }
}