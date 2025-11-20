using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public static bool GameIsPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resumes game time
        GameIsPaused = false;
        // Optional: unlock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Stops game time/physics
        GameIsPaused = true;
        // Optional: unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        // Set time back to normal before loading new scene
        Time.timeScale = 1f;
        // Reloads the current scene by name or index
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        // Set time back to normal before loading new scene
        Time.timeScale = 1f;
        // Load the main menu scene (ensure it's added to Build Settings)
        SceneManager.LoadScene("Main_Menu"); // Replace with your scene name
    }
}

