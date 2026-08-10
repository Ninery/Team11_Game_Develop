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

    private HashSet<string> heldItems = new HashSet<string>();

    void Awake()
    {
        Instance = this;

        foreach (var slot in slots)
            slot.icon.SetActive(false);
    }

    public void AddItem(string itemName)
    {
        heldItems.Add(itemName);
        SetIconVisible(itemName, true);
    }

    public void RemoveItem(string itemName)
    {
        heldItems.Remove(itemName);
        SetIconVisible(itemName, false);
    }

    public bool HasItem(string itemName)
    {
        return heldItems.Contains(itemName);
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