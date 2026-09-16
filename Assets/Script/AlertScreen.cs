using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class AlertScreen : MonoBehaviour
{
    [Header("Setup")]
    public string nextSceneName = "Tutorial";
    public float autoAdvanceDelay = 4f; 
    public AudioSource alarmAudioSource; 

    [Header("Typing")]
    public TMP_Text alertText; 
    [TextArea] public string message = "Monsters are on the loose in the city.";
    public float charactersPerSecond = 30f; 

    private bool isTyping = false;
    private bool advancing = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        if (alarmAudioSource != null)
            alarmAudioSource.Play();

        if (alertText != null)
            typingCoroutine = StartCoroutine(TypeText());
    }

    void Update()
    {

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isTyping)
            {
                SkipTyping();
            }
            else if (!advancing)
            {
                Advance();
            }
        }
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        alertText.text = "";

        float delay = 1f / charactersPerSecond;

        foreach (char c in message)
        {
            alertText.text += c;
            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
        StartAutoAdvanceTimer();
    }

    private void SkipTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        alertText.text = message;
        isTyping = false;
        StartAutoAdvanceTimer();
    }

    private void StartAutoAdvanceTimer()
    {
        if (autoAdvanceDelay > 0f)
            StartCoroutine(AutoAdvanceAfterDelay());
    }

    private IEnumerator AutoAdvanceAfterDelay()
    {
        yield return new WaitForSeconds(autoAdvanceDelay);
        Advance();
    }

    private void Advance()
    {
        if (advancing) return;
        advancing = true;

        if (SceneTransition.Instance != null)
            SceneTransition.Instance.GoToScene(nextSceneName);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}