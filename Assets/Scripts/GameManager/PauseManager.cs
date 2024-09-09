using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        SceneManager.UnloadSceneAsync("MainMenu");
    }
}
