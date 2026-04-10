using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI moneyText;

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

}