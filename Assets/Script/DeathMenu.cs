using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public GameObject deathMenuUI; 
    public static bool PlayerIsDead = false;

    void Start()
    {
        deathMenuUI.SetActive(false); 
    }

    public void ShowDeathMenu()
    {
        PlayerIsDead = true;
        deathMenuUI.SetActive(true);
        Time.timeScale = 0f;

        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main_Menu");
    }
}
