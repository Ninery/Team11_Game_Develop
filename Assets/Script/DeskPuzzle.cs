using UnityEngine;

public class DeskPuzzle : MonoBehaviour, IInteractable
{
    public KeypadPuzzle keypad;
    public Player player;

    [Header("On Solve")]
    public string itemToRemove = "CoatNote";
    public string itemToAdd = "KeyCard";

    private bool solved = false;

    public void Interact()
    {
        if (solved) return;

        keypad.OpenKeypad(player);
    }

    public void OnKeypadSolved()
    {
        Inventory.Instance.RemoveItem(itemToRemove);
        Inventory.Instance.AddItem(itemToAdd);

        DialogueManager.Instance.ShowDialogue("I found a keycard.");

        solved = true;
        GetComponent<Collider2D>().enabled = false;
    }
}