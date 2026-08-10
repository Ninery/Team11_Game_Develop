using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;

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
            onSolved.Invoke();
            ClosePuzzle();
        }
        else
        {
            enteredCode = "";
            codeDisplay.text = "";
        }
    }

    public void ClosePuzzle()
    {
        keypadCanvas.SetActive(false);
        currentPlayer.canMove = true;
    }
}