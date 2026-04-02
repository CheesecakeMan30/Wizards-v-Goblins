using UnityEngine;

public class WizardShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 2f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            Shoot();
            timer = 0;
        }
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }
}