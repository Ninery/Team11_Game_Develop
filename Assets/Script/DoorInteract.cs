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

    [Header("Sound")]
    public AudioSource lockedAudioSource;
    public AudioSource unlockAudioSource;

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
            if (lockedAudioSource != null && lockedAudioSource.clip != null)
                lockedAudioSource.PlayOneShot(lockedAudioSource.clip);

            DialogueManager.Instance.ShowDialogue("Locked.");
        }
    }
    
    public static void ResetOpenedDoors()
    {
        openedDoors.Clear();
    }

    private void UnlockDoor()
    {
        if (!string.IsNullOrEmpty(requiredItem))
            Inventory.Instance.RemoveItem(requiredItem);

        if (!string.IsNullOrEmpty(doorId))
            openedDoors.Add(doorId);

        if (unlockAudioSource != null && unlockAudioSource.clip != null)
            unlockAudioSource.PlayOneShot(unlockAudioSource.clip);

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