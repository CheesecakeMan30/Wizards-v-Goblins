using UnityEngine;

public class Castle : MonoBehaviour
{
    public static Castle instance;

    public int health = 10;

    void Awake()
    {
        instance = this;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log("Castle Health: " + health);

        if (health <= 0)
    {
        Debug.Log("Game Over");
        GameManager.instance.gameOver = true;
        Time.timeScale = 0;
    }
    }
}