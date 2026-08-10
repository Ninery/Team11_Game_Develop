using UnityEngine;

public class DoorKeypad : MonoBehaviour, IInteractable
{
    [Header("Keypad")]
    public KeypadPuzzle keypad;
    public Player player;

    [Header("Door Visuals")]
    public GameObject doorClosed;
    public GameObject doorOpen;

    private bool solved = false;

    public void Interact()
    {
        if (solved) return;

        keypad.OpenKeypad(player);
    }

    public void OnKeypadSolved()
    {
        solved = true;

        doorClosed.SetActive(false);
        doorOpen.SetActive(true);

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }
    }
}