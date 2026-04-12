using UnityEngine;
using System.Collections.Generic;

public class Wizard : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 1.5f;

    public int maxHealth = 5;
    public int health = 5;
    public int cost; 
    public float detectionRange = 8f;   // how far wizard can see
    public float shootStartX = 4f;      // lane start (your red line)

    float fireTimer = 0f;

    void Update()
    {
        if (!CanShoot()) return;

        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    bool CanShoot()
{
    GameObject[] goblins = GameObject.FindGameObjectsWithTag("Goblin");

    foreach (GameObject g in goblins)
    {
        if (g == null) continue;

        float dx = g.transform.position.x - transform.position.x;
        float dy = Mathf.Abs(g.transform.position.y - transform.position.y);

        // 🔥 MUST be same lane (VERY IMPORTANT)
        if (dy > 0.5f) continue;

        // 🔥 MUST be in front
        if (dx <= 0) continue;

        // 🔥 MUST be within range
        if (dx > detectionRange) continue;

        // 🔥 MUST have reached lane start
        if (g.transform.position.x > shootStartX) continue;

        return true;
    }

    return false;
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

    void OnMouseDown()
    {
    GameManager.instance.SelectWizard(gameObject);
    }
}