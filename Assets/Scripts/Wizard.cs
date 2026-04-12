using UnityEngine;
using System.Collections.Generic;

public class Wizard : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 1.5f;

    public int maxHealth = 5;
    public int health = 5;
    public int cost;

    public float detectionRange = 8f;
    public float shootStartX = 4f;

    public float attackDelay = 0.15f; //  different per wizard

    float fireTimer = 0f;

    Animator anim;
    bool isAttacking = false; // prevents spam

    void Start()
    {
        anim = GetComponent<Animator>();
    }

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

            if (dy > 0.5f) continue;
            if (dx <= 0) continue;
            if (dx > detectionRange) continue;
            if (g.transform.position.x > shootStartX) continue;

            return true;
        }

        return false;
    }

    void Shoot()
    {
        if (isAttacking) return;

        isAttacking = true;

        if (anim != null)
            anim.SetTrigger("Shoot");

        // OPTION A (delay-based)
        CancelInvoke(nameof(SpawnProjectile));
        Invoke(nameof(SpawnProjectile), attackDelay);

        // reset attack lock (match animation length)
        Invoke(nameof(ResetAttack), 0.5f);
    }

    void SpawnProjectile()
    {
        Instantiate(projectilePrefab, transform.position + Vector3.right * 0.5f, Quaternion.identity);
    }

    void ResetAttack()
    {
        isAttacking = false;
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