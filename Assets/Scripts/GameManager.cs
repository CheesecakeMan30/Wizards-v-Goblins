using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI moneyText;
    public GameObject waveCompleteUI;
    public GameObject winScreen;

    public GameObject sellButton;

    private GameObject selectedWizard;

    public int goblinsKilled = 0;
    public int goblinsAlive = 0;
    public int wave = 1;
    public bool gameOver = false;
    public int money = 2750;

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

   
 void Update()
{
    if (UnityEngine.EventSystems.EventSystem.current != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        return;
    if (Input.GetMouseButtonDown(0))
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int layerMask = ~LayerMask.GetMask("Goblin");
        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos, layerMask);

        GameObject clickedWizard = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Wizard"))
            {
                clickedWizard = hit.gameObject;
                break;
            }
        }

        if (clickedWizard != null && Time.timeScale == 0f)
        {
            SelectWizard(clickedWizard);
        }
        else
        {
            DeselectWizard();
        }
    }
}

    public void GoblinSpawned()
    {
        goblinsAlive++;
    }

    public void GoblinKilled()
    {
        goblinsKilled++;
        goblinsAlive--;
        money += 50;
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

    public void DeselectWizard()
    {
        selectedWizard = null;

        if (sellButton != null)
            sellButton.SetActive(false);
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
        DeselectWizard(); // 🔥 FIXED
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
        GamePause.instance.ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}