using UnityEngine;

public class WizardShoot : MonoBehaviour
{
    public GameObject projectilePrefab;

    public float fireRate = 1f;
    public Transform firePoint; // where projectile spawns

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
    }
}