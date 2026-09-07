using UnityEngine;
using System.Collections.Generic;

public class DoorInteract : MonoBehaviour, IInteractable
{
<<<<<<< Updated upstream
    [Header("Door Parent")]
    public GameObject doorIdentity;
=======
    [Header("Unique ID - type a distinct name per door")]
    public string doorId;
>>>>>>> Stashed changes

    [Header("Door Visuals")]
    public GameObject doorClosed;
    public GameObject doorOpen;

    [Header("Requirement (leave blank for no requirement)")]
    public string requiredItem = "";

<<<<<<< Updated upstream
    private static HashSet<int> openedDoors = new HashSet<int>();

    void Start()
    {
        if (doorIdentity != null && openedDoors.Contains(doorIdentity.GetInstanceID()))
=======
    private static HashSet<string> openedDoors = new HashSet<string>();

    void Start()
    {
        if (!string.IsNullOrEmpty(doorId) && openedDoors.Contains(doorId))
>>>>>>> Stashed changes
            ApplyOpenState();
    }

    public void Interact()
    {
<<<<<<< Updated upstream
        if (doorIdentity != null && openedDoors.Contains(doorIdentity.GetInstanceID()))
=======
        if (!string.IsNullOrEmpty(doorId) && openedDoors.Contains(doorId))
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        if (doorIdentity != null)
            openedDoors.Add(doorIdentity.GetInstanceID());
=======
        if (!string.IsNullOrEmpty(doorId))
            openedDoors.Add(doorId);
>>>>>>> Stashed changes

        ApplyOpenState();
    }

<<<<<<< Updated upstream
=======
        public void ForceOpen()
    {
        if (!string.IsNullOrEmpty(doorId))
            openedDoors.Add(doorId);

        ApplyOpenState();
    }

>>>>>>> Stashed changes
    private void ApplyOpenState()
    {
        doorClosed.SetActive(false);
        doorOpen.SetActive(true);

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;
    }
}