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
            // Deliberately NOT calling pauseAction.action.Disable() here.
            // "Pause" lives in the Project-Wide Input Actions asset -- one
            // shared instance for the whole game, not a copy per scene.
            // On a scene load, Unity runs the NEW scene's OnEnable() before
            // destroying the OLD scene's objects (and running their
            // OnDisable()). If we called Disable() here, it would run right
            // after the new scene's Enable() and silently turn Pause back
            // off -- which is exactly why it worked once and then stopped.
            // Leaving it enabled for the whole session avoids that race.
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

    // Hook this up to your "Restart" button's OnClick() -- reloads the
    // current scene, preserving anything meant to persist mid-game
    // (e.g. collected cats), matching Keyu's existing checkpoint-restart
    // behaviour. Deliberately does NOT touch CatManager.
    public void RestartFromCheckpoint()
    {
        Time.timeScale = 1f;
        isPaused = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Hook this up to your "Main Menu" button's OnClick()
    public void ReturnToMainMenu()
    {
        // Restore timescale BEFORE loading, same reasoning as DeathScreen --
        // otherwise the next scene can start frozen if timeScale is still 0.
        Time.timeScale = 1f;
        isPaused = false;

        // This is a full restart of the game from the very beginning, so
        // clear any collected cats -- unlike RestartFromCheckpoint() above,
        // which deliberately leaves cat progress untouched.
        if (CatManager.Instance != null)
            CatManager.Instance.ResetAllProgress();

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
