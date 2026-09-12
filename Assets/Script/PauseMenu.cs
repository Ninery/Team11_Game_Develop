using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [Header("References")]
    public GameObject pausePanel;   // the panel/canvas group that holds the pause UI
    public GameObject pauseButton;  // the always-visible on-screen button that opens the pause menu
    public string mainMenuSceneName = "StartMenu";

    [Header("Input")]
    public InputActionReference pauseAction; // drag the "Pause" action from InputSystem_Actions here

    private bool isPaused = false;

    void Awake()
    {
        // No DontDestroyOnLoad here on purpose -- unlike DeathScreen, the pause
        // menu should live inside the gameplay scene itself, not persist across
        // scene loads. If you later put it in its own scene loaded additively,
        // that's fine too, this script doesn't assume either way.
        Instance = this;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.Enable();
            pauseAction.action.performed += OnPausePerformed;
        }
    }

    void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPausePerformed;
            pauseAction.action.Disable();
        }
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        Toggle();
    }

    public void Toggle()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pausePanel != null)
            pausePanel.SetActive(true);
        if (pauseButton != null)
            pauseButton.SetActive(false);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null)
            pausePanel.SetActive(false);
        if (pauseButton != null)
            pauseButton.SetActive(true);
    }

    // Restart from the latest checkpoint
    public void RestartFromCheckpoint()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Return to StartMenu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}