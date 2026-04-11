using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI moneyText;
    public GameObject waveCompleteUI; // ✅ FIXED name

    public int goblinsKilled = 0;
    public int goblinsAlive = 0;
    public int wave = 1;
    public bool gameOver = false;
    public int money = 500;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateMoneyUI();
    }

    public void GoblinSpawned()
    {
        goblinsAlive++;
    }

    public void GoblinKilled()
    {
        goblinsKilled++;
        goblinsAlive--;
        money += 25;
        UpdateMoneyUI();

    }

    public void UpdateMoneyUI()
    {
        moneyText.text = "$" + money.ToString();
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyUI();
            return true;
        }
        return false;
    }

    public void LoseMoney(int amount)
    {
        money -= amount;
        if (money < 0) money = 0;
        UpdateMoneyUI();
    }

    public void ClearProjectiles()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (GameObject p in projectiles)
        {
            Destroy(p);
        }
    }
}