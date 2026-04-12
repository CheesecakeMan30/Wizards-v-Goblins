using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI moneyText;
    public GameObject waveCompleteUI;
    public GameObject winScreen;

    public GameObject sellButton; // NEW

    private GameObject selectedWizard; //  NEW

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

        if (sellButton != null)
            sellButton.SetActive(false);
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

    // SELECT WIZARD
    public void SelectWizard(GameObject wizard)
    {
        // only allow between rounds
        if (Time.timeScale > 0f) return;

        selectedWizard = wizard;

        if (sellButton != null)
            sellButton.SetActive(true);
    }

    // SELL WIZARD
    public void SellSelectedWizard()
    {
        if (selectedWizard == null) return;

        Wizard wizardScript = selectedWizard.GetComponent<Wizard>();

        if (wizardScript != null)
        {
            int refund = Mathf.RoundToInt(wizardScript.cost * 0.5f);
            money += refund;
            UpdateMoneyUI();
        }

        Destroy(selectedWizard);
        selectedWizard = null;

        if (sellButton != null)
            sellButton.SetActive(false);
    }

    // WIN SCREEN
    public void ShowWinScreen()
    {
        GamePause.instance.PauseGame();
        winScreen.SetActive(true);
    }

    public void ContinueAfterWin()
    {
        winScreen.SetActive(false);
        GamePause.instance.ResumeGame();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}