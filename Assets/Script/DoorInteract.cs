using UnityEngine;
using System.Collections.Generic;

public class DoorInteract : MonoBehaviour, IInteractable
{
    [Header("Unique ID - type a distinct name per door")]
    public string doorId;

    [Header("Door Visuals")]
    public GameObject doorClosed;
    public GameObject doorOpen;

    [Header("Requirement (leave blank for no requirement)")]
    public string requiredItem = "";

    private static HashSet<string> openedDoors = new HashSet<string>();

    void Start()
    {
        if (!string.IsNullOrEmpty(doorId) && openedDoors.Contains(doorId))
            ApplyOpenState();
    }

    public void Interact()
    {
        if (!string.IsNullOrEmpty(doorId) && openedDoors.Contains(doorId))
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

        if (!string.IsNullOrEmpty(doorId))
            openedDoors.Add(doorId);

        ApplyOpenState();
    }

        public void ForceOpen()
    {
        if (!string.IsNullOrEmpty(doorId))
            openedDoors.Add(doorId);

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