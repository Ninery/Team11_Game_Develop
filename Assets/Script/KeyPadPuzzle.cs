using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;
using System.Collections;

public class KeypadPuzzle : MonoBehaviour
{
    [Header("Setup")]
    public GameObject keypadCanvas;
    public TMP_Text codeDisplay;
    public GameObject placeholderText; // separate object, just toggled on/off
    public string correctCode = "1234";
    public int maxDigits = 4;

    [Header("On Success")]
    public UnityEvent onSolved;

    [Header("Auto Close")]
    public float autoCloseDelay = 1f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioSource correctAudioSource;
    public AudioSource wrongAudioSource;

    private string enteredCode = "";
    private bool hasEnteredAnything = false;
    private Player currentPlayer;

    public void OpenKeypad(Player player)
    {
        currentPlayer = player;
        currentPlayer.canMove = false;

        keypadCanvas.SetActive(true);
        enteredCode = "";
        codeDisplay.text = "";

        placeholderText.SetActive(!hasEnteredAnything);
    }

    public void PressDigit(string digit)
    {
        if (enteredCode.Length >= maxDigits) return;

        if (audioSource != null)
            audioSource.PlayOneShot(audioSource.clip);

        hasEnteredAnything = true;
        placeholderText.SetActive(false);

        enteredCode += digit;
        codeDisplay.text = enteredCode;
    }

    public void PressDelete()
    {
        if (enteredCode.Length > 0)
        {
            enteredCode = enteredCode.Substring(0, enteredCode.Length - 1);
            codeDisplay.text = enteredCode;

            if (enteredCode.Length == 0 && !hasEnteredAnything)
                placeholderText.SetActive(true);
        }
    }

    public void PressEnter()
    {
        if (enteredCode == correctCode)
        {
            if (correctAudioSource != null)
                correctAudioSource.PlayOneShot(correctAudioSource.clip);

            onSolved.Invoke();

            StartCoroutine(AutoCloseAfterDelay());
        }
        else
        {
            if (wrongAudioSource != null)
                wrongAudioSource.PlayOneShot(wrongAudioSource.clip);

            enteredCode = "";
            codeDisplay.text = "";
        }
    }

    private IEnumerator AutoCloseAfterDelay()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        ClosePuzzle();
    }

    public void ClosePuzzle()
    {
        keypadCanvas.SetActive(false);
        currentPlayer.canMove = true;
    }
}