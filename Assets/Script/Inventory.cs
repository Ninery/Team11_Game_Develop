using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class InventorySlot
{
    public string itemName;
    public GameObject icon;
}

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<InventorySlot> slots;
    public AudioSource audioSource;

    private HashSet<string> heldItems = new HashSet<string>();

    void Awake()
    {
        Instance = this;

        foreach (var slot in slots)
            slot.icon.SetActive(false);
    }

    void Start()
    {
        if (CheckpointManager.Instance != null &&
            CheckpointManager.Instance.TryGetCheckpoint(out _, out List<string> savedItems))
        {
            foreach (string item in savedItems)
                AddItem(item);
        }
    }

    public void AddItem(string itemName)
    {
        if (heldItems.Add(itemName))
        {
            SetIconVisible(itemName, true);

            if (audioSource != null && audioSource.clip != null)
                audioSource.PlayOneShot(audioSource.clip);
        }
    }

    public void RemoveItem(string itemName)
    {
        heldItems.Remove(itemName);
        SetIconVisible(itemName, false);

        if (audioSource != null)
            audioSource.PlayOneShot(audioSource.clip);
    }

    public bool HasItem(string itemName)
    {
        return heldItems.Contains(itemName);
    }

    public IEnumerable<string> GetHeldItems()
    {
        return heldItems;
    }

    private void SetIconVisible(string itemName, bool visible)
    {
        foreach (var slot in slots)
        {
            if (slot.itemName == itemName)
            {
                slot.icon.SetActive(visible);
                return;
            }
        }
    }
}