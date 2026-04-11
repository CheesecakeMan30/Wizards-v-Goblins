using UnityEngine;
using UnityEngine.UI; // 🔥 needed

public class GamePause : MonoBehaviour
{
    public static GamePause instance;

    public bool isPaused = false;

    public float normalSpeed = 1f;
    public float fastSpeed = 2f;

    private bool isFastForward = false;

    // 🔥 ADD THIS
    public Image fastForwardButtonImage;

    public Color normalColor = Color.white;
    public Color fastForwardColor = new Color(0.7f, 0.7f, 0.7f); // darker grey

    void Awake()
    {
        instance = this;
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = isFastForward ? fastSpeed : normalSpeed;
        isPaused = false;
    }

    public void ToggleFastForward()
    {
        if (isPaused) return;

        if (isFastForward)
        {
            Time.timeScale = normalSpeed;
            isFastForward = false;

            // set normal color
            fastForwardButtonImage.color = normalColor;
        }
        else
        {
            Time.timeScale = fastSpeed;
            isFastForward = true;

            // set darker color
            fastForwardButtonImage.color = fastForwardColor;
        }
    }
}