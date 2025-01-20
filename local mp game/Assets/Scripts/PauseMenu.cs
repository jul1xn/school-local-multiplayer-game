using UnityEngine;
using UnityEngine.SceneManagement; // For Scene Management (Main Menu, etc.)
using UnityEngine.UI; // For Button Handling

public class GamePauseManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas;

    public Button continueButton;
    public Button mainMenuButton;
    public Button quitButton;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuCanvas.SetActive(false);

        continueButton.onClick.AddListener(ContinueGame);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ContinueGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuCanvas.SetActive(true);
    }

    void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuCanvas.SetActive(false);
    }

    void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
