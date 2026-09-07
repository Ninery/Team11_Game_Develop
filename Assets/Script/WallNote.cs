using UnityEngine;
using System.Collections;

public class WallNote : MonoBehaviour, IInteractable
{
    [Header("Note UI")]
    public GameObject noteCanvas;
    public string codeMessage = "110101 for lift";

    [Header("Levers, left to right, matching the code order")]
    public LeverInteract[] levers;

    [Header("Code (1 = down, 0 = up), must match levers.Length")]
    public string correctCode = "110101";

    [Header("On Close")]
    public GameObject patrolMonster;
    public GameObject chaseMonster; // starts inactive, positioned above/off-screen ready to drop
    public float delayBeforeChase = 2f;

    private Player currentPlayer;

    public void Interact()
    {
        if (noteCanvas == null) return;

        currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        currentPlayer.canMove = false;

        noteCanvas.SetActive(true);
    }

    public void CloseNote()
    {
        if (currentPlayer != null)
            currentPlayer.canMove = true;

        if (noteCanvas != null)
            noteCanvas.SetActive(false);

        if (patrolMonster != null)
            patrolMonster.SetActive(false);

        StartCoroutine(DelayedChaseStart());
    }

    private IEnumerator DelayedChaseStart()
    {
        yield return new WaitForSeconds(delayBeforeChase);

        if (chaseMonster != null)
            chaseMonster.SetActive(true);
    }

    public bool AreLeversCorrect()
    {
        if (levers.Length != correctCode.Length)
        {
            Debug.LogWarning("Lever count doesn't match code length!");
            return false;
        }

        for (int i = 0; i < levers.Length; i++)
        {
            bool expectedDown = correctCode[i] == '1';
            if (levers[i].IsDown() != expectedDown)
                return false;
        }

        return true;
    }
}