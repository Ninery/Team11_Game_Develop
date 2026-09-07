using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LightsOutPuzzle : MonoBehaviour
{
    [Header("Green overlay Images, index 0-8 matching grid order")]
    public Image[] buttonOverlays = new Image[LightsOutManager.GridSize];

    [Header("Status Text (Locked/Unlocked)")]
    public TMP_Text statusText;
    public string lockedMessage = "Locked";
    public string unlockedMessage = "Unlocked";

    [Header("Auto Close")]
    public float autoCloseDelay = 2f;

    private Player currentPlayer;
    private Coroutine autoCloseCoroutine;

    void OnEnable()
    {
        RefreshVisuals();
        LightsOutManager.Instance.OnStateChanged += RefreshVisuals;
        LightsOutManager.Instance.OnSolved += HandleSolved;

        UpdateStatusText();

        currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        currentPlayer.canMove = false;
    }

    void OnDisable()
    {
        if (LightsOutManager.Instance != null)
        {
            LightsOutManager.Instance.OnStateChanged -= RefreshVisuals;
            LightsOutManager.Instance.OnSolved -= HandleSolved;
        }

        if (autoCloseCoroutine != null)
            StopCoroutine(autoCloseCoroutine);
    }

    public void PressButton(int index)
    {
        LightsOutManager.Instance.ToggleButton(index);
    }

    public void ClosePuzzle()
    {
        if (currentPlayer != null)
            currentPlayer.canMove = true;

        gameObject.SetActive(false);
    }

    private void HandleSolved()
    {
        UpdateStatusText();
        autoCloseCoroutine = StartCoroutine(AutoCloseAfterDelay());
    }

    private IEnumerator AutoCloseAfterDelay()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        ClosePuzzle();
    }

    private void UpdateStatusText()
    {
        if (statusText != null)
            statusText.text = LightsOutManager.Instance.doorOpen ? unlockedMessage : lockedMessage;
    }

    private void RefreshVisuals()
    {
        for (int i = 0; i < buttonOverlays.Length; i++)
        {
            Color c = buttonOverlays[i].color;
            c.a = LightsOutManager.Instance.buttonStates[i] ? 1f : 0f;
            buttonOverlays[i].color = c;
        }
    }

    public void ResetButtons()
    {
        LightsOutManager.Instance.ResetToStartingPattern();
    }
}