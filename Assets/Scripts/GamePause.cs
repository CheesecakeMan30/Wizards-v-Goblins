using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    public static GamePause instance;

    public bool isPaused = false;

    public float normalSpeed = 1f;
    public float fastSpeed = 2f;

    private bool isFastForward = false;

    public Image fastForwardButtonImage;

    public Color normalColor = Color.white;
    public Color fastForwardColor = new Color(0.7f, 0.7f, 0.7f);

    public GameObject pauseMenu;

    public bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    void ApplyTimeScale()
    {
        if (isPaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = isFastForward ? fastSpeed : normalSpeed;

        if (fastForwardButtonImage != null)
        {
            fastForwardButtonImage.color = isFastForward ? fastForwardColor : normalColor;
        }
    }

    public void TogglePause()
    {
        if (isGameOver) return;

        if (isPaused)
            ResumeGame();
        else
            PauseGame(true);
    }

    public void PauseGame(bool showMenu = true)
    {
        isPaused = true;
        ApplyTimeScale();

        if (pauseMenu != null && showMenu && !isGameOver)
            pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        ApplyTimeScale();

        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    public void ToggleFastForward()
    {
        if (isPaused || isGameOver) return;

        isFastForward = !isFastForward;
        ApplyTimeScale();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isFastForward = false;
        isGameOver = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}