using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{
    [Header("Door Visuals")]
    public GameObject doorClosed;
    public GameObject doorOpen;

    [Header("Requirement")]
    public string requiredItem = "KeyCard";

    private bool isUnlocked = false;

    public void Interact()
    {
        if (isUnlocked) return;

        if (Inventory.Instance.HasItem(requiredItem))
        {
            UnlockDoor();
        }
        else
        {
            DialogueManager.Instance.ShowDialogue("Locked.");
        }
    }

    private void UnlockDoor()
    {
        isUnlocked = true;

        Inventory.Instance.RemoveItem(requiredItem);

        doorClosed.SetActive(false);
        doorOpen.SetActive(true);

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }
    }
}