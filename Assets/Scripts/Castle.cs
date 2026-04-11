using UnityEngine;
using UnityEngine.SceneManagement;

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
        GameManager.instance.LoseMoney(75); // lose money when castle takes damage
        GameManager.instance.UpdateMoneyUI();



        if (health <= 0)
    {
        Debug.Log("Game Over");
        GameManager.instance.gameOver = true;
        SceneManager.LoadScene("End Screen");
        Time.timeScale = 0;
    }
    }
}