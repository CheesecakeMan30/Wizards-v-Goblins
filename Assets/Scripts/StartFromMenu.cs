using UnityEngine;
using UnityEngine.SceneManagement;

public class StartFromMenu : MonoBehaviour
{
    void Awake()
    {
#if UNITY_EDITOR
        // ONLY run if we pressed Play directly in Game scene
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            // Prevent looping by checking if we're already switching
            if (!SceneManager.GetSceneByBuildIndex(0).isLoaded)
            {
                SceneManager.LoadScene(0);
            }
        }
#endif
    }
}