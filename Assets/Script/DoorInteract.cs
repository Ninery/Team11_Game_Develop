using UnityEngine;
using System.Collections.Generic;

public class DoorInteract : MonoBehaviour, IInteractable
{
    [Header("Door Parent")]
    public GameObject doorIdentity;

    [Header("Door Visuals")]
    public GameObject doorClosed;
    public GameObject doorOpen;

    [Header("Requirement (leave blank for no requirement)")]
    public string requiredItem = "";

    private static HashSet<int> openedDoors = new HashSet<int>();

    void Start()
    {
        if (doorIdentity != null && openedDoors.Contains(doorIdentity.GetInstanceID()))
            ApplyOpenState();
    }

    public void Interact()
    {
        if (doorIdentity != null && openedDoors.Contains(doorIdentity.GetInstanceID()))
            return;

        bool hasRequirement = string.IsNullOrEmpty(requiredItem) || Inventory.Instance.HasItem(requiredItem);

        if (hasRequirement)
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
        if (!string.IsNullOrEmpty(requiredItem))
            Inventory.Instance.RemoveItem(requiredItem);

        if (doorIdentity != null)
            openedDoors.Add(doorIdentity.GetInstanceID());

        ApplyOpenState();
    }

    private void ApplyOpenState()
    {
        doorClosed.SetActive(false);
        doorOpen.SetActive(true);

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;
    }
}