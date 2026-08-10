using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("References")]
    public GameObject dialogueBox;
    public TMP_Text dialogueText;

    [Header("Typing Settings")]
    public float charactersPerSecond = 30f;

    [Header("Auto Close")]
    public float autoCloseDelay = 2f; // seconds after typing finishes before auto-closing

    private Coroutine typingCoroutine;
    private Coroutine autoCloseCoroutine;
    private bool isTyping = false;
    private string fullText = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            dialogueBox.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!dialogueBox.activeSelf) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isTyping)
            {
                SkipTyping();
            }
            else
            {
                CloseDialogue();
            }
        }
    }

    public void ShowDialogue(string message)
    {
        fullText = message;
        dialogueBox.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        if (autoCloseCoroutine != null)
            StopCoroutine(autoCloseCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        dialogueText.text = "";

        float delay = 1f / charactersPerSecond;

        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
        StartAutoCloseTimer();
    }

    private void SkipTyping()
    {
        StopCoroutine(typingCoroutine);
        dialogueText.text = fullText;
        isTyping = false;
        StartAutoCloseTimer();
    }

    private void StartAutoCloseTimer()
    {
        if (autoCloseCoroutine != null)
            StopCoroutine(autoCloseCoroutine);

        autoCloseCoroutine = StartCoroutine(AutoCloseAfterDelay());
    }

    private IEnumerator AutoCloseAfterDelay()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        CloseDialogue();
    }

    private void CloseDialogue()
    {
        if (autoCloseCoroutine != null)
            StopCoroutine(autoCloseCoroutine);

        dialogueBox.SetActive(false);
    }
}